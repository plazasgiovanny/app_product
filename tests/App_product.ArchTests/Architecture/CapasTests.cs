using NetArchTest.Rules;
using Xunit;

namespace App_product.ArchTests.Architecture;

/// <summary>
/// Pruebas de arquitectura que verifican las reglas de dependencia entre capas
/// de la arquitectura limpia implementada en el proyecto.
/// Estas pruebas actúan como guardrails automatizados para asegurar que
/// ningún desarrollador rompa las fronteras arquitectónicas por accidente.
/// </summary>
public class CapasTests
{
    // Namespaces raíz de cada capa
    private const string NsDomain         = "App_product.Domain";
    private const string NsApplication    = "App_product.Application";
    private const string NsInfrastructure = "App_product.Infrastructure";
    private const string NsApi            = "App_product.Api";

    // -------------------------------------------------------------------------
    // REGLA 1: Domain NO debe depender de ninguna otra capa del proyecto
    // -------------------------------------------------------------------------

    /// <summary>
    /// El núcleo del dominio debe ser completamente autónomo.
    /// No puede referenciar Application, Infrastructure ni Api.
    /// </summary>
    [Fact(DisplayName = "Regla 1 – Domain no depende de Application")]
    public void Domain_NoDebeDependeDe_Application()
    {
        var result = Types.InAssembly(typeof(App_product.Domain.Entities.Producto).Assembly)
            .ShouldNot()
            .HaveDependencyOn(NsApplication)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain tiene dependencias no permitidas en Application: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact(DisplayName = "Regla 2 – Domain no depende de Infrastructure")]
    public void Domain_NoDebeDependeDe_Infrastructure()
    {
        var result = Types.InAssembly(typeof(App_product.Domain.Entities.Producto).Assembly)
            .ShouldNot()
            .HaveDependencyOn(NsInfrastructure)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain tiene dependencias no permitidas en Infrastructure: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact(DisplayName = "Regla 3 – Domain no depende de Api")]
    public void Domain_NoDebeDependeDe_Api()
    {
        var result = Types.InAssembly(typeof(App_product.Domain.Entities.Producto).Assembly)
            .ShouldNot()
            .HaveDependencyOn(NsApi)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Domain tiene dependencias no permitidas en Api: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    // -------------------------------------------------------------------------
    // REGLA 4-5: Application NO debe depender de Infrastructure ni de Api
    // -------------------------------------------------------------------------

    /// <summary>
    /// La capa de aplicación orquesta casos de uso a través de interfaces;
    /// nunca debe conocer la implementación concreta de infraestructura.
    /// </summary>
    [Fact(DisplayName = "Regla 4 – Application no depende de Infrastructure")]
    public void Application_NoDebeDependeDe_Infrastructure()
    {
        var result = Types.InAssembly(typeof(App_product.Application.Services.IProductoService).Assembly)
            .ShouldNot()
            .HaveDependencyOn(NsInfrastructure)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Application tiene dependencias no permitidas en Infrastructure: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact(DisplayName = "Regla 5 – Application no depende de Api")]
    public void Application_NoDebeDependeDe_Api()
    {
        var result = Types.InAssembly(typeof(App_product.Application.Services.IProductoService).Assembly)
            .ShouldNot()
            .HaveDependencyOn(NsApi)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Application tiene dependencias no permitidas en Api: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    // -------------------------------------------------------------------------
    // REGLA 6: Infrastructure NO debe depender de Api
    // -------------------------------------------------------------------------

    [Fact(DisplayName = "Regla 6 – Infrastructure no depende de Api")]
    public void Infrastructure_NoDebeDependeDe_Api()
    {
        var result = Types.InAssembly(typeof(App_product.Infrastructure.Repositories.ProductoRepository).Assembly)
            .ShouldNot()
            .HaveDependencyOn(NsApi)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Infrastructure tiene dependencias no permitidas en Api: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    // -------------------------------------------------------------------------
    // REGLA 7: Los repositorios concretos deben vivir en Infrastructure
    // -------------------------------------------------------------------------

    /// <summary>
    /// Toda clase cuyo nombre termine en "Repository" debe residir en el
    /// namespace de Infrastructure. Las interfaces de repositorio en Domain
    /// son la excepción (comienzan con "I").
    /// </summary>
    [Fact(DisplayName = "Regla 7 – Implementaciones de repositorio están en Infrastructure")]
    public void Repositorios_Concretos_DebenEstar_EnInfrastructure()
    {
        var result = Types.InAssembly(typeof(App_product.Infrastructure.Repositories.ProductoRepository).Assembly)
            .That()
            .HaveNameEndingWith("Repository")
            .And()
            .AreNotInterfaces()
            .Should()
            .ResideInNamespaceStartingWith(NsInfrastructure)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Repositorios concretos fuera de Infrastructure: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    // -------------------------------------------------------------------------
    // REGLA 8: Los controladores deben vivir en la capa Api
    // -------------------------------------------------------------------------

    [Fact(DisplayName = "Regla 8 – Controladores están en Api")]
    public void Controladores_DebenEstar_EnApi()
    {
        var result = Types.InAssembly(typeof(App_product.Api.Controllers.V1.ProductosController).Assembly)
            .That()
            .HaveNameEndingWith("Controller")
            .Should()
            .ResideInNamespaceStartingWith(NsApi)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Controladores fuera de la capa Api: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    // -------------------------------------------------------------------------
    // REGLA 9: Los DTOs deben vivir en Application
    // -------------------------------------------------------------------------

    [Fact(DisplayName = "Regla 9 – DTOs están en Application")]
    public void Dtos_DebenEstar_EnApplication()
    {
        var result = Types.InAssembly(typeof(App_product.Application.Services.IProductoService).Assembly)
            .That()
            .HaveNameEndingWith("Dto")
            .Should()
            .ResideInNamespaceStartingWith(NsApplication)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"DTOs fuera de la capa Application: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    // -------------------------------------------------------------------------
    // REGLA 10: Las entidades de dominio deben vivir en Domain
    // -------------------------------------------------------------------------

    [Fact(DisplayName = "Regla 10 – Entidades de dominio están en Domain")]
    public void Entidades_DebenEstar_EnDomain()
    {
        var result = Types.InAssembly(typeof(App_product.Domain.Entities.Producto).Assembly)
            .That()
            .ResideInNamespace($"{NsDomain}.Entities")
            .Should()
            .ResideInNamespaceStartingWith(NsDomain)
            .GetResult();

        Assert.True(result.IsSuccessful,
            $"Entidades fuera de la capa Domain: " +
            $"{string.Join(", ", result.FailingTypeNames ?? [])}");
    }
}
