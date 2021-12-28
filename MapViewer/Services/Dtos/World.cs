namespace MapViewer.Services.Dtos
{
    using NPoco;

    [TableName("MapViewer.World")]
    [PrimaryKey("Id")]
    public class World
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime? RenderedDate { get; set; }

        public string? Name { get; set; }

        public string? BackupPath { get; set; }

        public string? OutputPath { get; set; }
    }
}
