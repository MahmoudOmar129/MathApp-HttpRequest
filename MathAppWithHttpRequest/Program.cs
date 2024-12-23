using System.Diagnostics.Eventing.Reader;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run( async (HttpContext Context) =>

{
    if (Context.Request.Method == "GET" && Context.Request.Path == "/")
    {
        int firstNumber = 0, secondNumber = 0;
        string? operation = null;
        long? result = null;

        //read 'firstNumber' if submitted in the request body
        if (Context.Request.Query.ContainsKey("firstNumber"))
        {
            string firstNumberString = Context.Request.Query["firstNumber"][0];
            if (!string.IsNullOrEmpty(firstNumberString))
            {
                firstNumber = Convert.ToInt32(firstNumberString);
            }
            else
            {
                Context.Response.StatusCode = 400;
                await Context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
            }
        }
        else
        {
            if (Context.Response.StatusCode == 200)
                Context.Response.StatusCode = 400;
            await Context.Response.WriteAsync("Invalid input for 'firstnumber'\n");
        }

        //read 'secondNumber' if submitted in the request body
        if (Context.Request.Query.ContainsKey("secondNumber"))
        {
            string secondNumberString = Context.Request.Query["secondNumber"][0];
            if (!string.IsNullOrEmpty(secondNumberString))
            {
                secondNumber = Convert.ToInt32(Context.Request.Query["secondNumber"][0]);
            }
            else
            {
                if (Context.Response.StatusCode == 200)
                        Context.Response.StatusCode = 400;
                await Context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
            }
        }
        else
        {
            if (Context.Response.StatusCode == 200)
                    Context.Response.StatusCode = 400;
            await Context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
        }

        //read 'operation' if submitted in the request body
        if (Context.Request.Query.ContainsKey("operation"))
        {
            operation = Convert.ToString(Context.Request.Query["operation"][0]);

            //perform the calculation based on the value of "operation"
            switch (operation)
            {
                case "add": result = firstNumber + secondNumber; break;
                case "subtract": result = firstNumber - secondNumber; break;
                case "multiply": result = firstNumber * secondNumber; break;
                case "divide": result = (secondNumber != 0) ? firstNumber / secondNumber : 0; break; //avoid DivideByZeroException, if secondNuber is 0 (zero)
                case "mod": result = (secondNumber != 0) ? firstNumber % secondNumber : 0; break; //avoid DivideByZeroException, if secondNuber is 0 (zero)
            }

            //If no case matched above, the "result" remains as 'null'
            if (result.HasValue)
            {
                await Context.Response.WriteAsync(result.Value.ToString());
            }

            //if invalid value is submitted for "operation" parameter
            else
            {
                if (Context.Response.StatusCode == 200)
                        Context.Response.StatusCode = 400;
                await Context.Response.WriteAsync("Invalid input for 'operation'\n");
            }

        } //end of "of ContainsKey("operation")

        //if the "operation" parameter is submitted from the client
        else
        {
            if (Context.Response.StatusCode == 200)
                    Context.Response.StatusCode = 400;
            await Context.Response.WriteAsync("Invalid input for 'operation'\n");
        }
    }
});

app.Run();