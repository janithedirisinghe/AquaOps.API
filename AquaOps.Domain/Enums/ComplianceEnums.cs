namespace AquaOps.Domain.Enums;

public enum CertificationType
{
    HACCP,
    FDA,
    BFAR,
    Halal,
    ISO22000,
    GMP,
    SSOP,
    EU_Compliant,
    Other
}

public enum DocumentType
{
    Certificate,
    License,
    Permit,
    Inspection,
    LabResult,
    TraceabilityReport,
    Other
}