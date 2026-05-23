namespace App_product.Api.GraphQL.Inputs;

/// <summary>
/// Datos de entrada para crear un <c>Producto</c> en el esquema GraphQL.
/// </summary>
[GraphQLName("ProductoCrearInput")]
public sealed record ProductoCrearInput(
    string Nombre,
    string? Descripcion,
    decimal Precio);
