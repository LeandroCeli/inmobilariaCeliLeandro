classDiagram

class Propietario {
  int Id
  string DNI
  string Apellido
  string Nombre
  string? Email
  string? Telefono
  string? Direccion
  DateTime FechaAlta
}

class Inquilino {
  int Id
  string DNI
  string Apellido
  string Nombre
  string? Email
  string? Telefono
  string? Direccion
  DateTime FechaAlta
}

class Inmueble {
  int Id
  string Direccion
  string Tipo
  string Uso
  int Ambientes
  decimal Precio
  int IdPropietario
  string? PropietarioNombre
}

class Contrato {
  int Id
  int IdPropiedad
  int IdInquilino
  DateTime FechaInicio
  DateTime FechaFin
  TipoOcupacion TipoOcupacion
  decimal MontoMensual
  decimal Deposito
  string PropiedadDireccion
  string InquilinoNombre
}

