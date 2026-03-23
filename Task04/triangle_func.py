class IncorrectTriangleSides(Exception):
    """Exception raised when triangle sides are invalid."""
    pass

def _validate_sides(a, b, c):
    """Validate triangle sides. Returns converted floats or raises IncorrectTriangleSides."""
    try:
        a = float(a)
        b = float(b)
        c = float(c)
    except (TypeError, ValueError):
        raise IncorrectTriangleSides("Sides must be numbers")

    if a <= 0 or b <= 0 or c <= 0:
        raise IncorrectTriangleSides("Sides must be positive")

    if a + b <= c or a + c <= b or b + c <= a:
        raise IncorrectTriangleSides("Triangle inequality violated")

    return a, b, c

def get_triangle_type(a, b, c):
    """Return triangle type as a string: 'equilateral', 'isosceles', or 'nonequilateral'.

    Raises IncorrectTriangleSides if sides are invalid.

    Examples:
    >>> get_triangle_type(2, 2, 2)
    'equilateral'
    >>> get_triangle_type(2, 3, 2)
    'isosceles'
    >>> get_triangle_type(3, 4, 5)
    'nonequilateral'
    >>> get_triangle_type(0, 1, 1)  # doctest: +ELLIPSIS
    Traceback (most recent call last):
        ...
    triangle_func.IncorrectTriangleSides: ...
    >>> get_triangle_type(1, 2, 3)  # doctest: +ELLIPSIS
    Traceback (most recent call last):
        ...
    triangle_func.IncorrectTriangleSides: ...
    """
    a, b, c = _validate_sides(a, b, c)

    if a == b == c:
        return "equilateral"
    elif a == b or b == c or a == c:
        return "isosceles"
    else:
        return "nonequilateral"
