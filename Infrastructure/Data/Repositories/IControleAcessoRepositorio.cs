﻿using despesas_backend_api_net_core.Domain.Entities;
using despesas_backend_api_net_core.Domain.VM;

namespace despesas_backend_api_net_core.Infrastructure.Data.Repositories
{
    public interface IControleAcessoRepositorio
    {
        bool Create(ControleAcesso controleAcesso);
        ControleAcesso FindByEmail(ControleAcesso controleAcesso);
        Usuario GetUsuarioByEmail(string login);
        bool ChangePassword(int idUsuario, string password);
        bool RecoveryPassword(string email);

    }
}
