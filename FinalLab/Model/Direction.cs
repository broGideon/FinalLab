namespace FinalLab.Model;

public class Direction
{
    public Direction(int specialityId, long oms)
    {
        SpecialityId = specialityId;
        Oms = oms;
    }

    public Direction()
    {
    }

    public int? IdDirection { get; set; }

    public int? SpecialityId { get; set; }

    public long? Oms { get; set; }
}