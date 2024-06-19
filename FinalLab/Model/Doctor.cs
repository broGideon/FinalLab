namespace FinalLab.Model;

public class Doctor
{
    public Doctor(string surname, string firstName, string patronymic, int specialityId, string enterPassword,
        string workAddress)
    {
        Surname = surname;
        FirstName = firstName;
        Patronymic = patronymic;
        SpecialityId = specialityId;
        EnterPassword = enterPassword;
        WorkAddress = workAddress;
    }

    public int? IdDoctor { get; set; }

    public string Surname { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public int? SpecialityId { get; set; }

    public string EnterPassword { get; set; } = null!;

    public string WorkAddress { get; set; } = null!;
}