namespace bugracker.Target
{
    enum TargetType
    {
        Mail,
        Directory
    }
    class Target
    {
        public TargetType  TargetType { get; set; }
        public string      Name { get; set; }
        public string      Path { get; set; }
    }
}
