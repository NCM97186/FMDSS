﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FMDSS.eSanchar {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", ConfigurationName="eSanchar.eSancharServiceSoap")]
    public interface eSancharServiceSoap {
        
        // CODEGEN: Generating message contract since message PostMessageRequest has headers
        [System.ServiceModel.OperationContractAttribute(Action="http://esanchar.rajasthan.gov.in/webservice/PostMessage", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BusinessEntityBase))]
        FMDSS.eSanchar.PostMessageResponse PostMessage(FMDSS.eSanchar.PostMessageRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://esanchar.rajasthan.gov.in/webservice/PostMessage", ReplyAction="*")]
        System.Threading.Tasks.Task<FMDSS.eSanchar.PostMessageResponse> PostMessageAsync(FMDSS.eSanchar.PostMessageRequest request);
        
        // CODEGEN: Generating message contract since message PostBulkMessagesXMLRequest has headers
        [System.ServiceModel.OperationContractAttribute(Action="http://esanchar.rajasthan.gov.in/webservice/PostBulkMessagesXML", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BusinessEntityBase))]
        FMDSS.eSanchar.PostBulkMessagesXMLResponse PostBulkMessagesXML(FMDSS.eSanchar.PostBulkMessagesXMLRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://esanchar.rajasthan.gov.in/webservice/PostBulkMessagesXML", ReplyAction="*")]
        System.Threading.Tasks.Task<FMDSS.eSanchar.PostBulkMessagesXMLResponse> PostBulkMessagesXMLAsync(FMDSS.eSanchar.PostBulkMessagesXMLRequest request);
        
        // CODEGEN: Generating message contract since message PostBulkMessagesEntityRequest has headers
        [System.ServiceModel.OperationContractAttribute(Action="http://esanchar.rajasthan.gov.in/webservice/PostBulkMessagesEntity", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BusinessEntityBase))]
        FMDSS.eSanchar.PostBulkMessagesEntityResponse PostBulkMessagesEntity(FMDSS.eSanchar.PostBulkMessagesEntityRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://esanchar.rajasthan.gov.in/webservice/PostBulkMessagesEntity", ReplyAction="*")]
        System.Threading.Tasks.Task<FMDSS.eSanchar.PostBulkMessagesEntityResponse> PostBulkMessagesEntityAsync(FMDSS.eSanchar.PostBulkMessagesEntityRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://esanchar.rajasthan.gov.in/webservice/PingService", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(BusinessEntityBase))]
        bool PingService();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://esanchar.rajasthan.gov.in/webservice/PingService", ReplyAction="*")]
        System.Threading.Tasks.Task<bool> PingServiceAsync();
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/")]
    public partial class AuthHeader : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string usernameField;
        
        private string passwordField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string Username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
                this.RaisePropertyChanged("Username");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
                this.RaisePropertyChanged("Password");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CampaignBulkMessageParameter))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CampaignBulkMessageRecordEntity))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CampaignBulkMessageEntity))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/")]
    public partial class BusinessEntityBase : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Nullable<int> createdByField;
        
        private System.Nullable<int> updatedByField;
        
        private System.Nullable<System.DateTime> createDateField;
        
        private System.Nullable<System.DateTime> updateDateField;
        
        private System.Nullable<bool> deleteFlagField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public System.Nullable<int> CreatedBy {
            get {
                return this.createdByField;
            }
            set {
                this.createdByField = value;
                this.RaisePropertyChanged("CreatedBy");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public System.Nullable<int> UpdatedBy {
            get {
                return this.updatedByField;
            }
            set {
                this.updatedByField = value;
                this.RaisePropertyChanged("UpdatedBy");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public System.Nullable<System.DateTime> CreateDate {
            get {
                return this.createDateField;
            }
            set {
                this.createDateField = value;
                this.RaisePropertyChanged("CreateDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public System.Nullable<System.DateTime> UpdateDate {
            get {
                return this.updateDateField;
            }
            set {
                this.updateDateField = value;
                this.RaisePropertyChanged("UpdateDate");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=4)]
        public System.Nullable<bool> DeleteFlag {
            get {
                return this.deleteFlagField;
            }
            set {
                this.deleteFlagField = value;
                this.RaisePropertyChanged("DeleteFlag");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/")]
    public partial class CampaignBulkMessageParameter : BusinessEntityBase {
        
        private string parameterKeyField;
        
        private string parameterValueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string ParameterKey {
            get {
                return this.parameterKeyField;
            }
            set {
                this.parameterKeyField = value;
                this.RaisePropertyChanged("ParameterKey");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ParameterValue {
            get {
                return this.parameterValueField;
            }
            set {
                this.parameterValueField = value;
                this.RaisePropertyChanged("ParameterValue");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/")]
    public partial class CampaignBulkMessageRecordEntity : BusinessEntityBase {
        
        private string phoneNumberField;
        
        private string messageField;
        
        private CampaignBulkMessageParameter[] messageParametersField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string PhoneNumber {
            get {
                return this.phoneNumberField;
            }
            set {
                this.phoneNumberField = value;
                this.RaisePropertyChanged("PhoneNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("Message");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=2)]
        public CampaignBulkMessageParameter[] MessageParameters {
            get {
                return this.messageParametersField;
            }
            set {
                this.messageParametersField = value;
                this.RaisePropertyChanged("MessageParameters");
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/")]
    public partial class CampaignBulkMessageEntity : BusinessEntityBase {
        
        private string campaignNameField;
        
        private string serviceNameField;
        
        private bool isCommonMessageField;
        
        private bool isParameterizedMessageField;
        
        private string commonMessageField;
        
        private CampaignBulkMessageRecordEntity[] campaignRecordsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string CampaignName {
            get {
                return this.campaignNameField;
            }
            set {
                this.campaignNameField = value;
                this.RaisePropertyChanged("CampaignName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ServiceName {
            get {
                return this.serviceNameField;
            }
            set {
                this.serviceNameField = value;
                this.RaisePropertyChanged("ServiceName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public bool IsCommonMessage {
            get {
                return this.isCommonMessageField;
            }
            set {
                this.isCommonMessageField = value;
                this.RaisePropertyChanged("IsCommonMessage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool IsParameterizedMessage {
            get {
                return this.isParameterizedMessageField;
            }
            set {
                this.isParameterizedMessageField = value;
                this.RaisePropertyChanged("IsParameterizedMessage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string CommonMessage {
            get {
                return this.commonMessageField;
            }
            set {
                this.commonMessageField = value;
                this.RaisePropertyChanged("CommonMessage");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=5)]
        public CampaignBulkMessageRecordEntity[] CampaignRecords {
            get {
                return this.campaignRecordsField;
            }
            set {
                this.campaignRecordsField = value;
                this.RaisePropertyChanged("CampaignRecords");
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PostMessage", WrapperNamespace="http://esanchar.rajasthan.gov.in/webservice/", IsWrapped=true)]
    public partial class PostMessageRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/")]
        public FMDSS.eSanchar.AuthHeader AuthHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=0)]
        public string serviceName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=1)]
        public string campaignName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=2)]
        public string phoneNo;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=3)]
        public string message;
        
        public PostMessageRequest() {
        }
        
        public PostMessageRequest(FMDSS.eSanchar.AuthHeader AuthHeader, string serviceName, string campaignName, string phoneNo, string message) {
            this.AuthHeader = AuthHeader;
            this.serviceName = serviceName;
            this.campaignName = campaignName;
            this.phoneNo = phoneNo;
            this.message = message;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PostMessageResponse", WrapperNamespace="http://esanchar.rajasthan.gov.in/webservice/", IsWrapped=true)]
    public partial class PostMessageResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=0)]
        public bool PostMessageResult;
        
        public PostMessageResponse() {
        }
        
        public PostMessageResponse(bool PostMessageResult) {
            this.PostMessageResult = PostMessageResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PostBulkMessagesXML", WrapperNamespace="http://esanchar.rajasthan.gov.in/webservice/", IsWrapped=true)]
    public partial class PostBulkMessagesXMLRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/")]
        public FMDSS.eSanchar.AuthHeader AuthHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=0)]
        public string messageXML;
        
        public PostBulkMessagesXMLRequest() {
        }
        
        public PostBulkMessagesXMLRequest(FMDSS.eSanchar.AuthHeader AuthHeader, string messageXML) {
            this.AuthHeader = AuthHeader;
            this.messageXML = messageXML;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PostBulkMessagesXMLResponse", WrapperNamespace="http://esanchar.rajasthan.gov.in/webservice/", IsWrapped=true)]
    public partial class PostBulkMessagesXMLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=0)]
        public bool PostBulkMessagesXMLResult;
        
        public PostBulkMessagesXMLResponse() {
        }
        
        public PostBulkMessagesXMLResponse(bool PostBulkMessagesXMLResult) {
            this.PostBulkMessagesXMLResult = PostBulkMessagesXMLResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PostBulkMessagesEntity", WrapperNamespace="http://esanchar.rajasthan.gov.in/webservice/", IsWrapped=true)]
    public partial class PostBulkMessagesEntityRequest {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/")]
        public FMDSS.eSanchar.AuthHeader AuthHeader;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=0)]
        public FMDSS.eSanchar.CampaignBulkMessageEntity campaignMessage;
        
        public PostBulkMessagesEntityRequest() {
        }
        
        public PostBulkMessagesEntityRequest(FMDSS.eSanchar.AuthHeader AuthHeader, FMDSS.eSanchar.CampaignBulkMessageEntity campaignMessage) {
            this.AuthHeader = AuthHeader;
            this.campaignMessage = campaignMessage;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="PostBulkMessagesEntityResponse", WrapperNamespace="http://esanchar.rajasthan.gov.in/webservice/", IsWrapped=true)]
    public partial class PostBulkMessagesEntityResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://esanchar.rajasthan.gov.in/webservice/", Order=0)]
        public bool PostBulkMessagesEntityResult;
        
        public PostBulkMessagesEntityResponse() {
        }
        
        public PostBulkMessagesEntityResponse(bool PostBulkMessagesEntityResult) {
            this.PostBulkMessagesEntityResult = PostBulkMessagesEntityResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface eSancharServiceSoapChannel : FMDSS.eSanchar.eSancharServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class eSancharServiceSoapClient : System.ServiceModel.ClientBase<FMDSS.eSanchar.eSancharServiceSoap>, FMDSS.eSanchar.eSancharServiceSoap {
        
        public eSancharServiceSoapClient() {
        }
        
        public eSancharServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public eSancharServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public eSancharServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public eSancharServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FMDSS.eSanchar.PostMessageResponse FMDSS.eSanchar.eSancharServiceSoap.PostMessage(FMDSS.eSanchar.PostMessageRequest request) {
            return base.Channel.PostMessage(request);
        }
        
        public bool PostMessage(FMDSS.eSanchar.AuthHeader AuthHeader, string serviceName, string campaignName, string phoneNo, string message) {
            FMDSS.eSanchar.PostMessageRequest inValue = new FMDSS.eSanchar.PostMessageRequest();
            inValue.AuthHeader = AuthHeader;
            inValue.serviceName = serviceName;
            inValue.campaignName = campaignName;
            inValue.phoneNo = phoneNo;
            inValue.message = message;
            FMDSS.eSanchar.PostMessageResponse retVal = ((FMDSS.eSanchar.eSancharServiceSoap)(this)).PostMessage(inValue);
            return retVal.PostMessageResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FMDSS.eSanchar.PostMessageResponse> FMDSS.eSanchar.eSancharServiceSoap.PostMessageAsync(FMDSS.eSanchar.PostMessageRequest request) {
            return base.Channel.PostMessageAsync(request);
        }
        
        public System.Threading.Tasks.Task<FMDSS.eSanchar.PostMessageResponse> PostMessageAsync(FMDSS.eSanchar.AuthHeader AuthHeader, string serviceName, string campaignName, string phoneNo, string message) {
            FMDSS.eSanchar.PostMessageRequest inValue = new FMDSS.eSanchar.PostMessageRequest();
            inValue.AuthHeader = AuthHeader;
            inValue.serviceName = serviceName;
            inValue.campaignName = campaignName;
            inValue.phoneNo = phoneNo;
            inValue.message = message;
            return ((FMDSS.eSanchar.eSancharServiceSoap)(this)).PostMessageAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FMDSS.eSanchar.PostBulkMessagesXMLResponse FMDSS.eSanchar.eSancharServiceSoap.PostBulkMessagesXML(FMDSS.eSanchar.PostBulkMessagesXMLRequest request) {
            return base.Channel.PostBulkMessagesXML(request);
        }
        
        public bool PostBulkMessagesXML(FMDSS.eSanchar.AuthHeader AuthHeader, string messageXML) {
            FMDSS.eSanchar.PostBulkMessagesXMLRequest inValue = new FMDSS.eSanchar.PostBulkMessagesXMLRequest();
            inValue.AuthHeader = AuthHeader;
            inValue.messageXML = messageXML;
            FMDSS.eSanchar.PostBulkMessagesXMLResponse retVal = ((FMDSS.eSanchar.eSancharServiceSoap)(this)).PostBulkMessagesXML(inValue);
            return retVal.PostBulkMessagesXMLResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FMDSS.eSanchar.PostBulkMessagesXMLResponse> FMDSS.eSanchar.eSancharServiceSoap.PostBulkMessagesXMLAsync(FMDSS.eSanchar.PostBulkMessagesXMLRequest request) {
            return base.Channel.PostBulkMessagesXMLAsync(request);
        }
        
        public System.Threading.Tasks.Task<FMDSS.eSanchar.PostBulkMessagesXMLResponse> PostBulkMessagesXMLAsync(FMDSS.eSanchar.AuthHeader AuthHeader, string messageXML) {
            FMDSS.eSanchar.PostBulkMessagesXMLRequest inValue = new FMDSS.eSanchar.PostBulkMessagesXMLRequest();
            inValue.AuthHeader = AuthHeader;
            inValue.messageXML = messageXML;
            return ((FMDSS.eSanchar.eSancharServiceSoap)(this)).PostBulkMessagesXMLAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        FMDSS.eSanchar.PostBulkMessagesEntityResponse FMDSS.eSanchar.eSancharServiceSoap.PostBulkMessagesEntity(FMDSS.eSanchar.PostBulkMessagesEntityRequest request) {
            return base.Channel.PostBulkMessagesEntity(request);
        }
        
        public bool PostBulkMessagesEntity(FMDSS.eSanchar.AuthHeader AuthHeader, FMDSS.eSanchar.CampaignBulkMessageEntity campaignMessage) {
            FMDSS.eSanchar.PostBulkMessagesEntityRequest inValue = new FMDSS.eSanchar.PostBulkMessagesEntityRequest();
            inValue.AuthHeader = AuthHeader;
            inValue.campaignMessage = campaignMessage;
            FMDSS.eSanchar.PostBulkMessagesEntityResponse retVal = ((FMDSS.eSanchar.eSancharServiceSoap)(this)).PostBulkMessagesEntity(inValue);
            return retVal.PostBulkMessagesEntityResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<FMDSS.eSanchar.PostBulkMessagesEntityResponse> FMDSS.eSanchar.eSancharServiceSoap.PostBulkMessagesEntityAsync(FMDSS.eSanchar.PostBulkMessagesEntityRequest request) {
            return base.Channel.PostBulkMessagesEntityAsync(request);
        }
        
        public System.Threading.Tasks.Task<FMDSS.eSanchar.PostBulkMessagesEntityResponse> PostBulkMessagesEntityAsync(FMDSS.eSanchar.AuthHeader AuthHeader, FMDSS.eSanchar.CampaignBulkMessageEntity campaignMessage) {
            FMDSS.eSanchar.PostBulkMessagesEntityRequest inValue = new FMDSS.eSanchar.PostBulkMessagesEntityRequest();
            inValue.AuthHeader = AuthHeader;
            inValue.campaignMessage = campaignMessage;
            return ((FMDSS.eSanchar.eSancharServiceSoap)(this)).PostBulkMessagesEntityAsync(inValue);
        }
        
        public bool PingService() {
            return base.Channel.PingService();
        }
        
        public System.Threading.Tasks.Task<bool> PingServiceAsync() {
            return base.Channel.PingServiceAsync();
        }
    }
}