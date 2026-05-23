namespace App_product.Api.GraphQL.Inputs;

/// <summary>
/// Datos de entrada para actualizar un <c>Producto</c> existente en el esquema GraphQL.
/// </summary>
[GraphQLName("ProductoActualizarInput")]
public sealed record ProductoActualizarInput(
    string Nombre,
    string? Descripcion,
    decimal Precio);
