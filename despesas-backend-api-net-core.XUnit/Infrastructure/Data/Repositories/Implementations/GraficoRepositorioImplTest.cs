﻿using despesas_backend_api_net_core.Infrastructure.Data.Repositories;

namespace Test.XUnit.Infrastructure.Data.Repositories.Implementations
{
    public class GraficoRepositorioImplTest
    {
        private readonly RegisterContext _context;
        private Mock<IGraficosRepositorio> _mockRepository;
        private Mock<GraficosRepositorioImpl> _repository;
        private Usuario _mockUsuario;
        private DateTime _mockAnoMes;
        public GraficoRepositorioImplTest()
        {
            _context = Usings.GetRegisterContext();
            _mockUsuario = UsuarioFaker.GetNewFaker();
            _context.Usuario.AddRange(_mockUsuario);
            var despesas = DespesaFaker.Despesas(_mockUsuario, _mockUsuario.Id);
            var receitas = ReceitaFaker.Receitas(_mockUsuario, _mockUsuario.Id);
            _mockAnoMes = despesas.First().Data;
            _context.Despesa.AddRange(despesas);
            _context.Receita.AddRange(receitas);
            _context.SaveChanges();
            _repository = new Mock<GraficosRepositorioImpl>(MockBehavior.Strict, _context);
            _mockRepository = Mock.Get<IGraficosRepositorio>(_repository.Object);
        }

        [Fact]
        public void GetDadosGraficoByAno_Should_Return_Grafico()
        {
            // Arrange
            var data = _mockAnoMes;
            var idUsuario = _mockUsuario.Id;

            // Act
            var result = _mockRepository.Object.GetDadosGraficoByAno(idUsuario, data);

            // Assert            
            Assert.NotNull(result);
            Assert.IsType<Grafico>(result);
            Assert.NotEmpty(result.SomatorioDespesasPorAno);
            Assert.NotEmpty(result.SomatorioReceitasPorAno);

        }

        [Fact]
        public void GetDadosGraficoByAno_Throws_Exception_And_Returns_Grafico_With_Default_Values()
        {
            // Arrange
            var idUsuario = 0;
            var data = _mockAnoMes;

            var despesaDbSetMock = new Mock<DbSet<Despesa>>();
            despesaDbSetMock.As<IQueryable<Despesa>>()
               .Setup(d => d.Provider)
               .Throws<Exception>();

            var options = new DbContextOptionsBuilder<RegisterContext>()
                .UseInMemoryDatabase(databaseName: "MemoryDatabase GetDadosGraficoByAno Throws Erro")
                .Options;

            var context = new RegisterContext(options);
            context.Despesa = despesaDbSetMock.Object;


            var repository = new GraficosRepositorioImpl(context);

            // Act
            var result = repository.GetDadosGraficoByAno(idUsuario, data);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Grafico>(result);
            Assert.NotEmpty(result.SomatorioDespesasPorAno);
            Assert.NotEmpty(result.SomatorioReceitasPorAno);
        }
    }
}