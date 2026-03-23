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

class Triangle:
    """Class representing a triangle with three sides.

    Attributes:
        a, b, c: Lengths of triangle sides (floats).
    """

    def __init__(self, a, b, c):
        """Initialize a triangle with three sides.

        Args:
            a, b, c: Lengths of triangle sides (must be positive numbers
                    satisfying triangle inequality).

        Raises:
            IncorrectTriangleSides: If sides are invalid.
        """
        self.a, self.b, self.c = _validate_sides(a, b, c)

    def triangle_type(self):
        """Return the type of triangle.

        Returns:
            'equilateral', 'isosceles', or 'nonequilateral'.
        """
        if self.a == self.b == self.c:
            return "equilateral"
        elif self.a == self.b or self.b == self.c or self.a == self.c:
            return "isosceles"
        else:
            return "nonequilateral"

    def perimeter(self):
        """Return the perimeter of the triangle."""
        return self.a + self.b + self.c
