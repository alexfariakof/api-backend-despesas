﻿using Business.Generic;
using Domain.Entities;
using Domain.VM;
using Repository.Mapping;
using Repository.Persistency.Generic;

namespace Business.Implementations;
public class DespesaBusinessImpl : IBusiness<DespesaVM>
{
    private readonly IRepositorio<Despesa> _repositorio;
    private readonly IRepositorio<Categoria> _repoCategoria;
    private readonly DespesaMap _converter;
    public DespesaBusinessImpl(IRepositorio<Despesa> repositorio, IRepositorio<Categoria> repoCategoria)
    {
        _repositorio = repositorio;
        _repoCategoria = repoCategoria;
        _converter = new DespesaMap();
    }
    public DespesaVM Create(DespesaVM obj)
    {
        IsCategoriaValid(obj);
        Despesa despesa = _converter.Parse(obj);
        _repositorio.Insert(ref despesa);
        return _converter.Parse(despesa);
    }

    public List<DespesaVM> FindAll(int idUsuario)
    {
        var despesas = _repositorio.GetAll().FindAll(d => d.UsuarioId == idUsuario);
        foreach( var despesa in despesas)
            despesa.Categoria = _repoCategoria.Get(despesa.CategoriaId);            
        return _converter.ParseList(despesas);
    }

    public DespesaVM FindById(int id, int idUsuario)
    {
        var despesa = _repositorio.Get(id);
        despesa.Categoria = _repoCategoria.Get(despesa.CategoriaId);
        var despesaVM = _converter.Parse(despesa);

        if (despesaVM.IdUsuario == idUsuario)
            return despesaVM;
        return null;
    }

    public DespesaVM Update(DespesaVM obj)
    {
        IsCategoriaValid(obj);
        Despesa despesa = _converter.Parse(obj);
        _repositorio.Update(ref despesa);
        return _converter.Parse(despesa);
    }

    public bool Delete(DespesaVM obj)
    {
        Despesa despesa = _converter.Parse(obj);
        return _repositorio.Delete(despesa);
    }
    private void IsCategoriaValid(DespesaVM obj)
    {
        if (_repoCategoria.GetAll().Find(c => c.UsuarioId == obj.IdUsuario && obj.Categoria.IdTipoCategoria == (int)TipoCategoria.Despesa) == null)
            throw new ArgumentException("Erro Categoria inexistente ou não cadastrada!");
    }
}