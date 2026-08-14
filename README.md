# Demant Technical Assessment

## Tech Stacks

- .NET 8 SDK
- xUnit


## Assumptions

- An empty `orderLines` list or A `null` order will behave same which yields a total of `0` rather than processing it as an error. A line with `quantity: 0` will not be treated as an error. Instead, system will compute every single order line as long as quantity value provided is a valid number. 
- The high-value discount threshold (10000) is evaluated only after application of volume discounts for each order line if any.
- Final amount is rounded to 2 decimal places with default .NET MidpointRounding.ToEven (banker's rounding). Ex: 10.505 -> 10.50.

