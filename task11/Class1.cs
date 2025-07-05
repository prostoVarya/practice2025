using System;
using System.Reflection;
using System.Reflection.Emit;

public class Program
{
    public static void Main()
    {
        Type calculatorType = CreateCalculatorType();
        
        object calculator = Activator.CreateInstance(calculatorType);
        
        TestCalculator(calculator);
    }
    
    public static Type CreateCalculatorType()
    {
        AssemblyName assemblyName = new AssemblyName("DynamicCalculator");
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        
        TypeBuilder typeBuilder = moduleBuilder.DefineType("Calculator", 
            TypeAttributes.Public | TypeAttributes.Class);
        
        DefineMethod(typeBuilder, "Add", typeof(int), new Type[] { typeof(int), typeof(int) });
        DefineMethod(typeBuilder, "Minus", typeof(int), new Type[] { typeof(int), typeof(int) });
        DefineMethod(typeBuilder, "Mul", typeof(int), new Type[] { typeof(int), typeof(int) });
        DefineMethod(typeBuilder, "Div", typeof(int), new Type[] { typeof(int), typeof(int) });
        
        return typeBuilder.CreateType();
    }
    
    private static void DefineMethod(TypeBuilder typeBuilder, string methodName, Type returnType, Type[] parameterTypes)
    {
        MethodBuilder methodBuilder = typeBuilder.DefineMethod(
            methodName,
            MethodAttributes.Public,
            returnType,
            parameterTypes);
        
        ILGenerator il = methodBuilder.GetILGenerator();
        
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldarg_2);
        
        switch (methodName)
        {
            case "Add":
                il.Emit(OpCodes.Add);
                break;
            case "Minus":
                il.Emit(OpCodes.Sub);
                break;
            case "Mul":
                il.Emit(OpCodes.Mul);
                break;
            case "Div":
                il.Emit(OpCodes.Div);
                break;
        }
        
        il.Emit(OpCodes.Ret);
    }
    
    public static void TestCalculator(object calculator)
    {
        dynamic calc = calculator;
        Console.WriteLine($"Add(5, 3): {calc.Add(5, 3)}");
        Console.WriteLine($"Minus(5, 3): {calc.Minus(5, 3)}");
        Console.WriteLine($"Mul(5, 3): {calc.Mul(5, 3)}");
        Console.WriteLine($"Div(6, 3): {calc.Div(6, 3)}");
    }
}
