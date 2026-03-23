"""Quadratic equation solver: ax^2 + bx + c = 0"""
import math

def solve_quadratic(a, b, c):
    """Solve quadratic equation ax^2 + bx + c = 0.

    Returns roots in ascending order.
    
    Args:
        a, b, c: Coefficients of the equation.

    Returns:
        tuple: Empty tuple if no real roots.
               Tuple with one element if one real root (double).
               Tuple with two elements if two distinct real roots (sorted ascending).

    Raises:
        ValueError: If a=0 and b=0 (not an equation).
    """
    try:
        a = float(a)
        b = float(b)
        c = float(c)
    except (TypeError, ValueError):
        raise ValueError("Coefficients must be numbers")

    # Handle linear equation: bx + c = 0
    if a == 0.0:
        if b == 0.0:
            raise ValueError("Not an equation (a=0 and b=0)")
        return (-c / b,)

    # Quadratic case: calculate discriminant
    discriminant = b * b - 4 * a * c

    if discriminant < 0:
        # No real roots
        return tuple()
    elif discriminant == 0:
        # One real root (double)
        x = -b / (2 * a)
        return (x,)
    else:
        # Two distinct real roots
        sqrt_d = math.sqrt(discriminant)
        x1 = (-b - sqrt_d) / (2 * a)
        x2 = (-b + sqrt_d) / (2 * a)
        return tuple(sorted([x1, x2]))


if __name__ == "__main__":
    # Test examples
    print("x^2 - 3x + 2 = 0:")
    print(solve_quadratic(1, -3, 2))  # (1.0, 2.0)

    print("\nx^2 + 2x + 1 = 0:")
    print(solve_quadratic(1, 2, 1))   # (-1.0,)

    print("\nx^2 + 1 = 0:")
    print(solve_quadratic(1, 0, 1))   # ()

    print("\n2x - 4 = 0:")
    print(solve_quadratic(0, 2, -4))  # (2.0,)
