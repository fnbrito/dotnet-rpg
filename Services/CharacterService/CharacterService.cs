using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new() {
            new Character(),
            new Character{ Id = 1, Name = "Jolton"},
        };
        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new();
            var character = _mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new();
            try
            {
                var character = characters.FirstOrDefault(c => c.Id == id);

                if (character is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                characters.Remove(character);

                serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDto>> serviceResponse = new();
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new();
            var character = characters.FirstOrDefault(x => x.Id == id)!;
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            if (serviceResponse.Data == null)
                serviceResponse.Success = false;
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> serviceResponse = new();
            try
            {
                var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

                if (character is null)
                    throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strength = updatedCharacter.Strength;
                character.Defense = updatedCharacter.Defense;
                character.Intelligence = updatedCharacter.Intelligence;
                character.Class = updatedCharacter.Class;

                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}