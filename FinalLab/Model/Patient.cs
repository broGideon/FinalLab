using SecondLibPractice;

namespace FinalLab.Model;

public class Patient : BindingHelper
{
    public long Oms { get; set; }

    public string Surname { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public DateOnly BirthDay { get; set; }

    public string AddressPatient { get; set; } = null!;

    private string? _livingAddress;

    public string? LivingAddress
    {
        get => _livingAddress; 
        set => SetField(ref _livingAddress, value);
    }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Nickname { get; set; }
}
