namespace FinalLab.Model;

public class Appointment
{
    public Appointment(int idAppointment, DateOnly appointmentDate, TimeOnly appointmentTime, long oms, int doctorId,
        int statusId)
    {
        IdAppointment = idAppointment;
        AppointmentDate = appointmentDate;
        AppointmentTime = appointmentTime;
        Oms = oms;
        StatusId = statusId;
        DoctorId = doctorId;
    }

    public Appointment(DateOnly appointmentDate, TimeOnly appointmentTime, long oms, int doctorId, int statusId)
    {
        AppointmentDate = appointmentDate;
        AppointmentTime = appointmentTime;
        Oms = oms;
        StatusId = statusId;
        DoctorId = doctorId;
    }
    
    public Appointment(){}

    public int? IdAppointment { get; set; }

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly AppointmentTime { get; set; }

    public int? DoctorId { get; set; }

    public long? Oms { get; set; }

    public int? StatusId { get; set; }
}