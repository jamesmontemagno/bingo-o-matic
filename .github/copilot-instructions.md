# Blazor WebAssembly Best Practices

## Project Structure
- Keep shared components in a `Shared` folder
- Keep Pages in the `Pages` folder
- Place interfaces in separate files

## Component Organization
1. Component files should follow the pattern:
   - `ComponentName.razor` - Component markup
   - `ComponentName.razor.css` - Scoped styles

## CSS Best Practices
- Use scoped CSS (`.razor.css` files) for component-specific styles
- Place global styles in `wwwroot/css/app.css`
- Follow BEM naming convention for CSS classes
- Use CSS variables for theming
- Prefer CSS Grid and Flexbox for layouts

## State Management
- Use cascading parameters for shared state
- Implement `IDisposable` for cleanup
- Use `@implements IAsyncDisposable` when needed
- Prefer injectable services for global state

## Performance
- Implement `@implements IDisposable` to clean up resources
- Use `@key` directive for dynamic content
- Implement virtualization for large lists
- Lazy load components when possible
- Use `ShouldRender()` to optimize rendering

## Memory Management & Leak Prevention
### IDisposable/IAsyncDisposable Patterns
- Always dispose `SemaphoreSlim` in try-finally blocks within DisposeAsync
- Use disposal guard flags (`_disposed`) to prevent multiple disposals
- Implement `IAsyncDisposable` for components using `CancellationTokenSource`
- Dispose `CancellationTokenSource` in components (especially for animations/timers)
- Call `Cancel()` before `Dispose()` on CancellationTokenSource

### JavaScript Interop Cleanup
- Always dispose `DotNetObjectReference` instances in Dispose methods
- Track event listeners in JavaScript and provide cleanup functions
- Call JavaScript cleanup functions before disposing DotNetObjectReference
- Use Map or similar structures to track handlers for proper cleanup
- Example pattern:
  ```javascript
  const _handlers = new Map();
  function setupListener(element, event, handler) {
    _handlers.set(`${event}-${element.id}`, handler);
    element.addEventListener(event, handler);
  }
  function cleanup() {
    _handlers.forEach((handler, key) => {
      const [event, elementId] = key.split('-');
      document.getElementById(elementId)?.removeEventListener(event, handler);
    });
    _handlers.clear();
  }
  ```

### Async Operation Management
- Avoid fire-and-forget async operations (`_ = SomeAsyncMethod()`)
- Use `Task.Run()` with proper error handling for background operations
- Always handle exceptions in background tasks
- Use `CancellationToken` for long-running operations
- Example safe pattern:
  ```csharp
  Task.Run(async () => {
    try {
      await SomeAsyncOperation();
    } catch (Exception ex) {
      // Log error appropriately
    }
  });
  ```

### Event Handler Cleanup
- Remove all DOM event listeners in cleanup functions
- Use AbortController for modern event listener cleanup when possible
- Clean up MutationObserver instances
- Remove global event handlers (window, document) on component disposal

### Service Disposal Best Practices
- Implement disposal guards in all public methods of disposable services
- Use ConfigureAwait(false) in disposal methods when possible
- Dispose resources in reverse order of creation
- Handle disposal of injected services appropriately

## Error Handling
- Implement error boundaries
- Use try-catch blocks in lifecycle methods
- Log errors to browser console in development
- Implement proper error UI for production

## Forms
- Use EditForm with DataAnnotations
- Implement custom validation when needed
- Use typed InputBase<T> components
- Handle form submission asynchronously

## JavaScript Interop
- Wrap JS calls in C# methods
- Implement proper disposal of JS references
- Use IJSRuntime for JS interop
- Prefer C# implementations when possible

## PWA Guidelines
- Test offline functionality
- Implement proper caching strategies
- Handle service worker updates
- Use proper meta tags for PWA

## Testing
- Unit test components using bUnit
- Mock JavaScript interop
- Test component lifecycle methods
- Verify component state changes

## Code Style
- Use C# 12 features appropriately
- Follow Microsoft's C# coding conventions
- Use meaningful names for components and methods
- Document public APIs with XML comments

## Security
- Validate all inputs
- Sanitize output when using `@Html.Raw`
- Implement proper authentication/authorization
- Use HTTPS in production

## Accessibility
- Use semantic HTML elements
- Implement ARIA attributes correctly
- Ensure keyboard navigation works
- Test with screen readers

Remember to always consider the end-user experience and maintain clean, maintainable code.
