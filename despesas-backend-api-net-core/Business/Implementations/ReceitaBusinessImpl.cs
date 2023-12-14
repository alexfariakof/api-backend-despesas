﻿using despesas_backend_api_net_core.Business.Generic;
using despesas_backend_api_net_core.Domain.Entities;
using despesas_backend_api_net_core.Domain.VM;
using despesas_backend_api_net_core.Infrastructure.Data.EntityConfig;
using despesas_backend_api_net_core.Infrastructure.Data.Repositories.Generic;

namespace despesas_backend_api_net_core.Business.Implementations
{
    public class ReceitaBusinessImpl : IBusiness<ReceitaVM>
    {
        private readonly IRepositorio<Receita> _repositorio;
        private readonly IRepositorio<Categoria> _repoCategoria;
        private readonly ReceitaMap _converter;

        public ReceitaBusinessImpl(IRepositorio<Receita> repositorio, IRepositorio<Categoria> repoCategoria)
        {
            _repositorio = repositorio;
            _repoCategoria = repoCategoria;
            _converter = new ReceitaMap();
            _repoCategoria = repoCategoria;
        }
        public ReceitaVM Create(ReceitaVM obj)
        {
            if (!IsCategoriaValid(obj))
                throw new Exception("Categoria não existe cadastrada para este usuário!");

            Receita receita = _repositorio.Insert(_converter.Parse(obj));
            return _converter.Parse(receita);
        }

        public List<ReceitaVM> FindAll(int idUsuario)
        {
            var receitas = _repositorio.GetAll().FindAll(d => d.UsuarioId == idUsuario);
            foreach (var receita in receitas)
                receita.Categoria = _repoCategoria.Get(receita.CategoriaId);
            return _converter.ParseList(receitas);
        }      

        public ReceitaVM FindById(int id, int idUsuario)
        {
            var receita = _repositorio.Get(id);
            receita.Categoria = _repoCategoria.Get(receita.CategoriaId);
            var receitaVm = _converter.Parse(receita);
            if (receitaVm.IdUsuario == idUsuario)
                return receitaVm;
            return null;
        }

        public ReceitaVM Update(ReceitaVM obj)
        {
            if (!IsCategoriaValid(obj))
                throw new Exception("Categoria não existe cadastrada para este usuário!");

            Receita receita = _repositorio.Update(_converter.Parse(obj));
            return _converter.Parse(receita);

        }

        public bool Delete(ReceitaVM obj)
        {
            Receita receita = _repositorio.Update(_converter.Parse(obj));
            return  _repositorio.Delete(receita);
        }

        private bool IsCategoriaValid(ReceitaVM obj)
        {
            return _repoCategoria.GetAll().Find(c => c.UsuarioId == obj.IdUsuario) != null ? true : false;
        }
    }
}
