﻿namespace Common.Validation;

public enum ValidationErrorCode
{
    INVALID_EMAIL,
    MISSING_REQUIRED_FIELDS,
    INVALID_CREDENTIALS,
    INVALID_COMPANY_IDENTIFIER,
    INVALID_POSTAL_CODE,
    COMPANY_NOT_FOUND,
    CANNOT_DELETE_COMPANY_WITH_INVOICES,
    INVOICE_NOT_FOUND,
    INVOICE_REQUIRES_NAME,
    INVOICE_LINE_REQUIRES_NAME,
    INVOICE_LINE_AMOUNT_MUST_BE_GREATER_THAN_ZERO,
    INVOICE_PAYMENT_DATE_MUST_BE_AFTER_PUBLISH_DATE,
    INVOICE_SELLER_AND_BUYER_SHOULD_BE_DIFFERENT,
    INVOICE_LINE_TAX_PERCENTAGE_MUST_BE_POSITIVE_NUMBER,
    INVOICE_LINE_UNIT_PRICE_MUST_BE_POSITIVE_NUMBER,
    INVOICE_WITH_GIVEN_NAME_ALREADY_EXISTS
}