namespace FinalLab.Model;

public class AnalysDocument
{
    public AnalysDocument(int idAppointment, string rtf, string documentName)
    {
        IdAppointment = idAppointment;
        Rtf = rtf;
        DocumentName = documentName;
    }
    
    public int? IdAppointment { get; set; }

    public string Rtf { get; set; } = null!;
    
    public string DocumentName { get; set; } = null!;
}
