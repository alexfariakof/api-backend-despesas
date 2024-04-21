﻿namespace XUnit.Fakers;
public class DespesaFaker
{
    static int counter = 1;
    static int counterVM = 1;

    private static DespesaFaker? _instance;
    private static readonly object LockObject = new object();
    public static DespesaFaker Instance
    {
        get
        {
            lock (LockObject)
            {
                return _instance ??= new DespesaFaker();
            }
        }
    }

    public Despesa GetNewFaker(Usuario usuario, Categoria categoria)
    {
        var despesaFaker = new Faker<Despesa>()
            .RuleFor(r => r.Id, f => counter++)
            .RuleFor(r => r.Data, new DateTime(DateTime.Now.Year, new Random().Next(1, 13), 1))
            .RuleFor(
                r => r.DataVencimento,
                new DateTime(DateTime.Now.Year, new Random().Next(1, 13), 1)
            )
            .RuleFor(r => r.Descricao, f => f.Commerce.ProductName())
            .RuleFor(r => r.Valor, f => f.Random.Decimal(1, 900000))
            .RuleFor(r => r.UsuarioId, usuario.Id)
            .RuleFor(r => r.Usuario, usuario)
            .RuleFor(r => r.CategoriaId, categoria.Id)
            .RuleFor(r => r.Categoria, CategoriaFaker.Instance.GetNewFaker(usuario, TipoCategoria.Despesa, usuario.Id));

        return despesaFaker.Generate();
    }

    public DespesaDto GetNewFakerVM(int idUsuario, int idCategoria)
    {
        var despesaFaker = new Faker<DespesaDto>()
            .RuleFor(r => r.Id, f => counterVM++)
            .RuleFor(r => r.Data, new DateTime(DateTime.Now.Year, new Random().Next(1, 13), 1))
            .RuleFor(
                r => r.DataVencimento,
                new DateTime(DateTime.Now.Year, new Random().Next(1, 13), 1)
            )
            .RuleFor(r => r.Descricao, f => f.Commerce.ProductName())
            .RuleFor(r => r.Valor, f => f.Random.Decimal(1, 900000))
            .RuleFor(r => r.IdUsuario, idUsuario)
            .RuleFor(r => r.Categoria, CategoriaFaker.Instance.GetNewFakerVM(UsuarioFaker.Instance.GetNewFakerVM(idUsuario), TipoCategoria.Despesa, idUsuario));

        return despesaFaker.Generate();
    }

    public List<DespesaDto> DespesasVMs(UsuarioDto? usuarioDto = null, int? idUsuario = null)
    {
        var listDespesaDto = new List<DespesaDto>();
        for (int i = 0; i < 10; i++)
        {
            if (idUsuario == null)
                usuarioDto = UsuarioFaker.Instance.GetNewFakerVM(new Random().Next(1, 10));

            var categoriaDto = CategoriaFaker.Instance.GetNewFakerVM(usuarioDto, TipoCategoria.Despesa);

            var despesaDto = GetNewFakerVM(usuarioDto.Id, categoriaDto.Id);
            listDespesaDto.Add(despesaDto);
        }

        return listDespesaDto;
    }

    public List<Despesa> Despesas(Usuario? usuario = null, int? idUsurio = null, int count = 10)
    {
        var listDespesa = new List<Despesa>();
        for (int i = 0; i < count; i++)
        {
            if (idUsurio == null)
                usuario = UsuarioFaker.Instance.GetNewFaker(new Random().Next(1, 10));

            var categoria = CategoriaFaker.Instance.GetNewFaker(usuario, TipoCategoria.Despesa, usuario.Id);

            var despesa = GetNewFaker(usuario, categoria);
            listDespesa.Add(despesa);
        }
        return listDespesa;
    }
}
