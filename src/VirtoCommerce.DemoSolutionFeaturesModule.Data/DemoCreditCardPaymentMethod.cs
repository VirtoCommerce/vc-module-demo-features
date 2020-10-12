using System;
using System.Collections.Specialized;
using VirtoCommerce.DemoSolutionFeaturesModule.Core;
using VirtoCommerce.PaymentModule.Core.Model;
using VirtoCommerce.PaymentModule.Model.Requests;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data
{
    public class DemoCreditCardPaymentMethod : PaymentMethod
    {
        public DemoCreditCardPaymentMethod()
            : base("DemoCreditCardPaymentMethod")
        {
        }

        [Obsolete("Need to use localized strings on clients side instead")]
        public new string Name => "Credit Card";

        public new string LogoUrl => Settings?.GetSettingValue(
            ModuleConstants.Settings.General.DemoCreditCardPaymentMethodLogo.Name,
            ModuleConstants.Settings.General.DemoCreditCardPaymentMethodLogo.DefaultValue.ToString()
        );


        public override PaymentMethodType PaymentMethodType => PaymentMethodType.Unknown;

        public override PaymentMethodGroupType PaymentMethodGroupType => PaymentMethodGroupType.BankCard;

        public override CapturePaymentRequestResult CaptureProcessPayment(CapturePaymentRequest context)
        {
            return new CapturePaymentRequestResult { IsSuccess = true, NewPaymentStatus = PaymentStatus.Paid };
        }

        public override PostProcessPaymentRequestResult PostProcessPayment(PostProcessPaymentRequest request)
        {
            throw new NotImplementedException();
        }

        public override ProcessPaymentRequestResult ProcessPayment(ProcessPaymentRequest request)
        {
            return new ProcessPaymentRequestResult { IsSuccess = true, NewPaymentStatus = PaymentStatus.Paid };
        }

        public override RefundPaymentRequestResult RefundProcessPayment(RefundPaymentRequest context)
        {
            throw new NotImplementedException();
        }

        public override ValidatePostProcessRequestResult ValidatePostProcessRequest(NameValueCollection queryString)
        {
            return new ValidatePostProcessRequestResult { IsSuccess = false };
        }

        public override VoidPaymentRequestResult VoidProcessPayment(VoidPaymentRequest request)
        {
            return new VoidPaymentRequestResult { IsSuccess = true, NewPaymentStatus = PaymentStatus.Voided };
        }
    }
}
