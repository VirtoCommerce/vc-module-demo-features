using System;
using VirtoCommerce.PaymentModule.Core.Model;
using VirtoCommerce.PaymentModule.Model.Requests;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data
{
    public class DemoInvoicePaymentMethod : PaymentMethod
    {
        public DemoInvoicePaymentMethod() : base("InvoicePaymentMethod")
        {
        }

        [Obsolete("Need to use localized strings on clients side instead")]
        public string Name => "Invoice";

        public override PaymentMethodType PaymentMethodType => PaymentMethodType.Unknown;

        public override PaymentMethodGroupType PaymentMethodGroupType => PaymentMethodGroupType.Manual;

        public override ProcessPaymentRequestResult ProcessPayment(ProcessPaymentRequest request)
        {
            return new ProcessPaymentRequestResult { IsSuccess = true, NewPaymentStatus = PaymentStatus.Paid };
        }

        public override PostProcessPaymentRequestResult PostProcessPayment(PostProcessPaymentRequest request)
        {
            throw new NotImplementedException();
        }

        public override VoidPaymentRequestResult VoidProcessPayment(VoidPaymentRequest request)
        {
            return new VoidPaymentRequestResult { IsSuccess = true, NewPaymentStatus = PaymentStatus.Voided };
        }

        public override CapturePaymentRequestResult CaptureProcessPayment(CapturePaymentRequest request)
        {
            return new CapturePaymentRequestResult { IsSuccess = true, NewPaymentStatus = PaymentStatus.Paid };
        }

        public override RefundPaymentRequestResult RefundProcessPayment(RefundPaymentRequest request)
        {
            throw new NotImplementedException();
        }

        public override ValidatePostProcessRequestResult ValidatePostProcessRequest(System.Collections.Specialized.NameValueCollection queryString)
        {
            return new ValidatePostProcessRequestResult { IsSuccess = false };
        }
    }
}
