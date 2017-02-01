namespace zh.fang.data.entity
{
    public class Config: DeleteableEntity
    {
        public string Name { get; set; }

        public string Data { get; set; }

        public short Type { get; set; }
    }
}
