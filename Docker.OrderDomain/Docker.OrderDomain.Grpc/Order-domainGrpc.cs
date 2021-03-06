// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: order-domain.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Docker.OrderDomain.Grpc {
  public static partial class OrderService
  {
    static readonly string __ServiceName = "Docker.OrderDomain.Grpc.OrderService";

    static readonly grpc::Marshaller<global::Docker.OrderDomain.Grpc.SendOrderRequest> __Marshaller_Docker_OrderDomain_Grpc_SendOrderRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Docker.OrderDomain.Grpc.SendOrderRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Docker.OrderDomain.Grpc.SendOrderReply> __Marshaller_Docker_OrderDomain_Grpc_SendOrderReply = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Docker.OrderDomain.Grpc.SendOrderReply.Parser.ParseFrom);

    static readonly grpc::Method<global::Docker.OrderDomain.Grpc.SendOrderRequest, global::Docker.OrderDomain.Grpc.SendOrderReply> __Method_SendOrder = new grpc::Method<global::Docker.OrderDomain.Grpc.SendOrderRequest, global::Docker.OrderDomain.Grpc.SendOrderReply>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "SendOrder",
        __Marshaller_Docker_OrderDomain_Grpc_SendOrderRequest,
        __Marshaller_Docker_OrderDomain_Grpc_SendOrderReply);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Docker.OrderDomain.Grpc.OrderDomainReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of OrderService</summary>
    public abstract partial class OrderServiceBase
    {
      public virtual global::System.Threading.Tasks.Task SendOrder(grpc::IAsyncStreamReader<global::Docker.OrderDomain.Grpc.SendOrderRequest> requestStream, grpc::IServerStreamWriter<global::Docker.OrderDomain.Grpc.SendOrderReply> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for OrderService</summary>
    public partial class OrderServiceClient : grpc::ClientBase<OrderServiceClient>
    {
      /// <summary>Creates a new client for OrderService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public OrderServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for OrderService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public OrderServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected OrderServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected OrderServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual grpc::AsyncDuplexStreamingCall<global::Docker.OrderDomain.Grpc.SendOrderRequest, global::Docker.OrderDomain.Grpc.SendOrderReply> SendOrder(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SendOrder(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncDuplexStreamingCall<global::Docker.OrderDomain.Grpc.SendOrderRequest, global::Docker.OrderDomain.Grpc.SendOrderReply> SendOrder(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_SendOrder, null, options);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override OrderServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new OrderServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(OrderServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SendOrder, serviceImpl.SendOrder).Build();
    }

  }
}
#endregion
