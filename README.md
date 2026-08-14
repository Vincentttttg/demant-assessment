# Demant Technical Assessment

## Tech stacks

- .NET 8 SDK
- ASP.NET Core Web API
- xUnit


## Assumptions

- An empty `orderLines` list or `null` order will behave same which yields a total of `0` rather than processing it as an error. A line with `quantity: 0` will not be treated as an error. Instead, system will compute every single order line as long as the provided quantity value is a valid numeric number. 
- The high-value discount threshold (10000) is evaluated only after application of volume discounts for each order line if any.
- Final amount is rounded to 2 decimal places with default .NET MidpointRounding.ToEven (banker's rounding). Ex: 10.505 -> 10.50.

## Design decisions

- **Dependency Injection**. Rather than instantiating CalculatorService in the controller, I registered it in Program.cs with a scoped lifetime, so one instance is created per HTTP request. The controller depends on ICalculatorService, not the concrete class, so swapping in a different pricing implementation is a one line change in Program.cs and the controller stays untouched. This design choice was primarily made to satisfy "ability to adapt to changing business rules."
- **Open for extension, closed for modification**. Instead of hardcoding the discount rules inside CalculatorService, I put them behind an IPricingRule interface and run them as a pipeline. Adding a new discount, say a loyalty discount or one scoped to ProductType, means writing a new rule and registering it in Program.cs. The existing rules and the service itself stay untouched, so there is nothing to break.
- **`unitPrice` non-negativity guard is enforced at the model layer** via  `[Range]` validation with the data annotation validator on `OrderLine.UnitPrice`. Since rule 2 states that unitPrice is always non-negative, this guard ensures that the value is always valid. Invalid model state is automatically rejected with `400 Bad Request` before It hits the action method. It introduces clean code separation, no manual validation code needed in the controller or service.

## Potential improvements

- **`Quantity` is not currently validated for negativity**. A negative quantity is accepted and silently reduces the order total instead of being rejected. This should get the same `[Range]` treatment as `UnitPrice`.
- `ProductType` could become an enum for better readability if pricing rules start differentiating by product type.
- Discount thresholds and percentages are hardcoded constants in each rule. If these need to change without a redeploy each time it is changed, it can be moved to json configuration file like `appsettings.json`.

