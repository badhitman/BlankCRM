﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @FakeGov 
////////////////////////////////////////////////

using System.Security.Authentication;
using System;
using System.ComponentModel.DataAnnotations;

namespace SharedLib;

/// <summary>
/// FIX message adapter.
/// </summary>
public partial class FixMessageAdapterModelDB : IBaseStockSharpModel
{
    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public int Id { get; set; }


    /// <inheritdoc/>
    public DateTime LastUpdatedAtUTC { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAtUTC { get; set; }


    /// <summary>
    /// Имя адаптера (например: LuaFixMarketDataMessageAdapter или LuaFixTransactionMessageAdapter и т.п.)
    /// </summary>
    public AdaptersTypesNames AdapterTypeName { get; set; }

    /// <inheritdoc/>
    public string Name { get; set; }

    /// <summary>
    /// Деактивирован?
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// Should the sequence counter be reset.
    /// </summary>
    public bool IsResetCounter { get; set; }

    /// <inheritdoc/>
    public bool IsDemo { get; set; }

    /// <inheritdoc/>
    public string Password { get; set; }

    /// <inheritdoc/>
    public string Login { get; set; }

    /// <inheritdoc/>
    public string TargetCompId { get; set; }

    /// <summary>
    /// Date format.
    /// </summary>
    public string DateFormat { get; set; }

    /// <inheritdoc/>
    public string SenderCompId { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Timestamp format.
    /// </summary>
    public string TimeStampFormat { get; set; }

    /// <summary>
    /// Time format.
    /// </summary>
    public string TimeFormat { get; set; }

    /// <summary>
    /// Year month format.
    /// </summary>
    public string YearMonthFormat { get; set; }

    /// <inheritdoc/>
    public string ClientVersion { get; set; }

    /// <summary>
    /// Override exec id by native identifier (if present in FIX message).
    /// </summary>
    public bool OverrideExecIdByNative { get; set; }

    /// <summary>
    /// Do not send StockSharp.Fix.Native.FixTags.Account.
    /// </summary>
    public bool DoNotSendAccount { get; set; }

    /// <summary>
    /// Cancel On Disconnect.
    /// </summary>
    public bool CancelOnDisconnect { get; set; }

    /// <summary>
    /// Support executions processing, generated by third-party software.
    /// </summary>
    public bool SupportUnknownExecutions { get; set; }

    /// <summary>
    /// The name of the server that shares SSL connection.
    /// </summary>
    public string TargetHost { get; set; }

    /// <summary>
    /// Validate remove certificates.
    /// </summary>
    public bool ValidateRemoteCertificates { get; set; }

    /// <summary>
    /// Check certificate revocation.
    /// </summary>
    public bool CheckCertificateRevocation { get; set; }

    /// <summary>
    /// SSL certificate.
    /// </summary>
    public string SslCertificate { get; set; }

    /// <summary>
    /// SSL protocol to establish connect.
    /// </summary>
    public SslProtocols SslProtocol { get; set; }

    /// <summary>
    /// Client code assigned by the broker.
    /// </summary>
    public string ClientCode { get; set; }

    /// <summary>
    /// Board, where securities are traded.
    /// </summary>
    public string ExchangeBoard { get; set; }

    /// <summary>
    /// The timeout of sending data. The default value is System.TimeSpan.Zero.
    /// </summary>
    public TimeSpan WriteTimeout { get; set; }

    /// <summary>
    /// The timeout of reading data. The default value is System.TimeSpan.Zero.
    /// </summary>
    public TimeSpan ReadTimeout { get; set; }

    /// <summary>
    /// Accounts associated with FIX login.
    /// </summary>
    public string Accounts { get; set; }

    /// <inheritdoc/>
    public bool EnqueueSubscriptions { get; set; }

    /// <inheritdoc/>
    public static FixMessageAdapterModelDB BuildEmpty()
    {
        return new FixMessageAdapterModelDB();
    }

    /// <inheritdoc/>
    public void SetUpdate(FixMessageAdapterModelDB other)
    {
        LastUpdatedAtUTC = DateTime.UtcNow;

        AdapterTypeName = other.AdapterTypeName;
        Name = other.Name;
        IsOnline = other.IsOnline;
        IsResetCounter = other.IsResetCounter;
        IsDemo = other.IsDemo;
        Password = other.Password;
        Login = other.Login;
        TargetCompId = other.TargetCompId;
        DateFormat = other.DateFormat;
        SenderCompId = other.SenderCompId;
        Address = other.Address;
        TimeStampFormat = other.TimeStampFormat;
        TimeFormat = other.TimeFormat;
        YearMonthFormat = other.YearMonthFormat;
        ClientVersion = other.ClientVersion;
        OverrideExecIdByNative = other.OverrideExecIdByNative;
        DoNotSendAccount = other.DoNotSendAccount;
        CancelOnDisconnect = other.CancelOnDisconnect;
        SupportUnknownExecutions = other.SupportUnknownExecutions;
        TargetHost = other.TargetHost;
        ValidateRemoteCertificates = other.ValidateRemoteCertificates;
        CheckCertificateRevocation = other.CheckCertificateRevocation;
        SslCertificate = other.SslCertificate;
        SslProtocol = other.SslProtocol;
        ClientCode = other.ClientCode;
        ExchangeBoard = other.ExchangeBoard;
        WriteTimeout = other.WriteTimeout;
        ReadTimeout = other.ReadTimeout;
        Accounts = other.Accounts;
        EnqueueSubscriptions = other.EnqueueSubscriptions;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return $"{Name}`{CreatedAtUTC}`{Password}`{WriteTimeout}`{Login}`{IsDemo}`{TargetCompId}`{IsOnline}`{DateFormat}`{AdapterTypeName}`{SenderCompId}`{DoNotSendAccount}`{Address}`{TimeStampFormat}`{TimeFormat}`{Id}`{YearMonthFormat}`{LastUpdatedAtUTC}`{IsResetCounter}`{ClientVersion}`{OverrideExecIdByNative}`{CancelOnDisconnect}`{SupportUnknownExecutions}`{TargetHost}`{ValidateRemoteCertificates}`{CheckCertificateRevocation}`{SslCertificate}`{SslProtocol}`{ClientCode}`{EnqueueSubscriptions}`{ExchangeBoard}`{ReadTimeout}`{Accounts}".GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (obj is FixMessageAdapterModelDB other)
        {
            return Id == other.Id &&
            LastUpdatedAtUTC == other.LastUpdatedAtUTC &&
            CreatedAtUTC == other.CreatedAtUTC &&
            AdapterTypeName == other.AdapterTypeName &&
            Name == other.Name &&
            IsOnline == other.IsOnline &&
            IsResetCounter == other.IsResetCounter &&
            IsDemo == other.IsDemo &&
            Password == other.Password &&
            Login == other.Login &&
            TargetCompId == other.TargetCompId &&
            DateFormat == other.DateFormat &&
            SenderCompId == other.SenderCompId &&
            Address == other.Address &&
            TimeStampFormat == other.TimeStampFormat &&
            TimeFormat == other.TimeFormat &&
            YearMonthFormat == other.YearMonthFormat &&
            ClientVersion == other.ClientVersion &&
            OverrideExecIdByNative == other.OverrideExecIdByNative &&
            DoNotSendAccount == other.DoNotSendAccount &&
            CancelOnDisconnect == other.CancelOnDisconnect &&
            SupportUnknownExecutions == other.SupportUnknownExecutions &&
            TargetHost == other.TargetHost &&
            ValidateRemoteCertificates == other.ValidateRemoteCertificates &&
            CheckCertificateRevocation == other.CheckCertificateRevocation &&
            SslCertificate == other.SslCertificate &&
            SslProtocol == other.SslProtocol &&
            ClientCode == other.ClientCode &&
            ExchangeBoard == other.ExchangeBoard &&
            WriteTimeout == other.WriteTimeout &&
            ReadTimeout == other.ReadTimeout &&
            Accounts == other.Accounts &&
            EnqueueSubscriptions == other.EnqueueSubscriptions;
        }

        return base.Equals(obj);
    }

    /// <inheritdoc/>
    public static bool operator ==(FixMessageAdapterModelDB a, FixMessageAdapterModelDB b)
    {
        if (a is null && b is null)
            return true;
        else if (a is null || b is null)
            return false;


        return a.Equals(b);
    }

    /// <inheritdoc/>
    public static bool operator !=(FixMessageAdapterModelDB a, FixMessageAdapterModelDB b)
    {
        if (a is null && b is null)
            return false;
        else if (a is null || b is null)
            return true;

        return a.Id != b.Id ||
           a.LastUpdatedAtUTC != b.LastUpdatedAtUTC ||
           a.CreatedAtUTC != b.CreatedAtUTC ||
           a.AdapterTypeName != b.AdapterTypeName ||
           a.Name != b.Name ||
           a.IsOnline != b.IsOnline ||
           a.IsResetCounter != b.IsResetCounter ||
           a.IsDemo != b.IsDemo ||
           a.Password != b.Password ||
           a.Login != b.Login ||
           a.TargetCompId != b.TargetCompId ||
           a.DateFormat != b.DateFormat ||
           a.SenderCompId != b.SenderCompId ||
           a.Address != b.Address ||
           a.TimeStampFormat != b.TimeStampFormat ||
           a.TimeFormat != b.TimeFormat ||
           a.YearMonthFormat != b.YearMonthFormat ||
           a.ClientVersion != b.ClientVersion ||
           a.OverrideExecIdByNative != b.OverrideExecIdByNative ||
           a.DoNotSendAccount != b.DoNotSendAccount ||
           a.CancelOnDisconnect != b.CancelOnDisconnect ||
           a.SupportUnknownExecutions != b.SupportUnknownExecutions ||
           a.TargetHost != b.TargetHost ||
           a.ValidateRemoteCertificates != b.ValidateRemoteCertificates ||
           a.CheckCertificateRevocation != b.CheckCertificateRevocation ||
           a.SslCertificate != b.SslCertificate ||
           a.SslProtocol != b.SslProtocol ||
           a.ClientCode != b.ClientCode ||
           a.ExchangeBoard != b.ExchangeBoard ||
           a.WriteTimeout != b.WriteTimeout ||
           a.ReadTimeout != b.ReadTimeout ||
           a.Accounts != b.Accounts ||
           a.EnqueueSubscriptions != b.EnqueueSubscriptions;
    }
}