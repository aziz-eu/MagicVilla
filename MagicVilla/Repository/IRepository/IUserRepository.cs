﻿using MagicVilla.Models;
using MagicVilla.Models.DTO;

namespace MagicVilla.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser (string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register (RegistrationRequestDTO registrationRequestDTO);
    }
}
