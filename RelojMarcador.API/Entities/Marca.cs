namespace RelojMarcador.API.Entities
{
    public class Marca
    {
        public int ID_Marca { get; set; }
        public int ID_Funcionario { get; set; }   
        public int ID_Area { get; set; }         
        public string? Detalle { get; set; }
        public string Tipo_Marca { get; set; } = string.Empty; //Entrada o Salida
        public DateTime Fecha_Hora { get; set; }
    }
}

