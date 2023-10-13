﻿using ChawlaClinic.BL.DTOs.Payment;

namespace ChawlaClinic.BL.ServiceInterfaces
{
    public interface IPaymentServiceRepo
    {
        bool AddPayment(AddPaymentDTO dto);
        List<GetPaymentDTO>? GetPayments(string patientId);
        GetPaymentDTO? GetPaymentById(string paymentId);
    }
}
