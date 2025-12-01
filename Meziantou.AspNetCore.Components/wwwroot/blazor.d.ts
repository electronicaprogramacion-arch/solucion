// Type definitions for Blazor WebAssembly JavaScript interop
// This file provides TypeScript definitions for the DotNet namespace

declare global {
  namespace DotNet {
    interface DotNetObject {
      invokeMethodAsync(methodName: string, ...args: any[]): Promise<any>;
      invokeMethod(methodName: string, ...args: any[]): any;
      dispose(): void;
    }

    interface DotNetObjectReference<T> extends DotNetObject {
    }

    function createJSObjectReference(jsObject: any): any;
    function disposeJSObjectReference(jsObjectReference: any): void;
  }

  interface Window {
    DotNet: typeof DotNet;
  }
}

export {};
