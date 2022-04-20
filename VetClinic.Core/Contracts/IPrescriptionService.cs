using VetClinic.Core.Models.Prescriptions;

namespace VetClinic.Core.Contracts
{
    public interface IPrescriptionService
    {
        void Create(
                DateTime createdOn,
                string description,
                string appointmentId);

        PrescriptionServiceModel Details(string id);

        IEnumerable<PrescriptionServiceModel> GetPrescriptionsByPet(string petId);

        IEnumerable<PrescriptionServiceModel> GetMine(string doctorId);
    }
}
