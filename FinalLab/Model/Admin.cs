namespace FinalLab.Model;

public class Admin
{
    public Admin(string surnameAdmin, string firstName, string patronymic, string enterPassword)
    {
        SurnameAdmin = surnameAdmin;
        FirstName = firstName;
        Patronymic = patronymic;
        EnterPassword = enterPassword;
    }
    public int? IdAdmin { get; set; }

    public string SurnameAdmin { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string EnterPassword { get; set; } = null!;
}
