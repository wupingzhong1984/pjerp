//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Enterprise.IIS.U8.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="U8.ServiceReference.U2ImportSoap")]
    public interface U2ImportSoap {
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 sXML 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Importvouch", ReplyAction="*")]
        Enterprise.IIS.U8.ServiceReference.ImportvouchResponse Importvouch(Enterprise.IIS.U8.ServiceReference.ImportvouchRequest request);
        
        // CODEGEN: 命名空间 http://tempuri.org/ 的元素名称 sXML 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Helloworld", ReplyAction="*")]
        Enterprise.IIS.U8.ServiceReference.HelloworldResponse Helloworld(Enterprise.IIS.U8.ServiceReference.HelloworldRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ImportvouchRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Importvouch", Namespace="http://tempuri.org/", Order=0)]
        public Enterprise.IIS.U8.ServiceReference.ImportvouchRequestBody Body;
        
        public ImportvouchRequest() {
        }
        
        public ImportvouchRequest(Enterprise.IIS.U8.ServiceReference.ImportvouchRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ImportvouchRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string sXML;
        
        public ImportvouchRequestBody() {
        }
        
        public ImportvouchRequestBody(string sXML) {
            this.sXML = sXML;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ImportvouchResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ImportvouchResponse", Namespace="http://tempuri.org/", Order=0)]
        public Enterprise.IIS.U8.ServiceReference.ImportvouchResponseBody Body;
        
        public ImportvouchResponse() {
        }
        
        public ImportvouchResponse(Enterprise.IIS.U8.ServiceReference.ImportvouchResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class ImportvouchResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string ImportvouchResult;
        
        public ImportvouchResponseBody() {
        }
        
        public ImportvouchResponseBody(string ImportvouchResult) {
            this.ImportvouchResult = ImportvouchResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloworldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Helloworld", Namespace="http://tempuri.org/", Order=0)]
        public Enterprise.IIS.U8.ServiceReference.HelloworldRequestBody Body;
        
        public HelloworldRequest() {
        }
        
        public HelloworldRequest(Enterprise.IIS.U8.ServiceReference.HelloworldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloworldRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string sXML;
        
        public HelloworldRequestBody() {
        }
        
        public HelloworldRequestBody(string sXML) {
            this.sXML = sXML;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloworldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloworldResponse", Namespace="http://tempuri.org/", Order=0)]
        public Enterprise.IIS.U8.ServiceReference.HelloworldResponseBody Body;
        
        public HelloworldResponse() {
        }
        
        public HelloworldResponse(Enterprise.IIS.U8.ServiceReference.HelloworldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class HelloworldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloworldResult;
        
        public HelloworldResponseBody() {
        }
        
        public HelloworldResponseBody(string HelloworldResult) {
            this.HelloworldResult = HelloworldResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface U2ImportSoapChannel : Enterprise.IIS.U8.ServiceReference.U2ImportSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class U2ImportSoapClient : System.ServiceModel.ClientBase<Enterprise.IIS.U8.ServiceReference.U2ImportSoap>, Enterprise.IIS.U8.ServiceReference.U2ImportSoap {
        
        public U2ImportSoapClient() {
        }
        
        public U2ImportSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public U2ImportSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public U2ImportSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public U2ImportSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Enterprise.IIS.U8.ServiceReference.ImportvouchResponse Enterprise.IIS.U8.ServiceReference.U2ImportSoap.Importvouch(Enterprise.IIS.U8.ServiceReference.ImportvouchRequest request) {
            return base.Channel.Importvouch(request);
        }
        
        public string Importvouch(string sXML) {
            Enterprise.IIS.U8.ServiceReference.ImportvouchRequest inValue = new Enterprise.IIS.U8.ServiceReference.ImportvouchRequest();
            inValue.Body = new Enterprise.IIS.U8.ServiceReference.ImportvouchRequestBody();
            inValue.Body.sXML = sXML;
            Enterprise.IIS.U8.ServiceReference.ImportvouchResponse retVal = ((Enterprise.IIS.U8.ServiceReference.U2ImportSoap)(this)).Importvouch(inValue);
            return retVal.Body.ImportvouchResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Enterprise.IIS.U8.ServiceReference.HelloworldResponse Enterprise.IIS.U8.ServiceReference.U2ImportSoap.Helloworld(Enterprise.IIS.U8.ServiceReference.HelloworldRequest request) {
            return base.Channel.Helloworld(request);
        }
        
        public string Helloworld(string sXML) {
            Enterprise.IIS.U8.ServiceReference.HelloworldRequest inValue = new Enterprise.IIS.U8.ServiceReference.HelloworldRequest();
            inValue.Body = new Enterprise.IIS.U8.ServiceReference.HelloworldRequestBody();
            inValue.Body.sXML = sXML;
            Enterprise.IIS.U8.ServiceReference.HelloworldResponse retVal = ((Enterprise.IIS.U8.ServiceReference.U2ImportSoap)(this)).Helloworld(inValue);
            return retVal.Body.HelloworldResult;
        }
    }
}
