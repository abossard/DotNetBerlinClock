def convert_string_to_timetuple(input_string):
    hours, minutes, seconds = map(int, input_string.split(":"))
    return hours, minutes, seconds


def convert_timetuple_to_berlin_data(hours, minutes, seconds):
    return int(hours / 5), hours % 5, int(minutes / 5), minutes % 5, seconds % 2 == 0


def create_format_func(positions, character="R", transpose=lambda index: False, transpose_character="R"):
    return lambda v: "".join([transpose_character if transpose(index) else character for index in range(v)]) + "O" * (
            positions - v)


format_hours = create_format_func(4)
format_minutes = create_format_func(11, "Y", lambda index: (index + 1) % 3 == 0)
format_minutes_1 = create_format_func(4, "Y")


def format_berlin_data(hours_5, hours_1, minutes_5, minutes_1, seconds_half):
    return "\n".join([
        "Y" if seconds_half else "O",
        format_hours(hours_5),
        format_hours(hours_1),
        format_minutes(minutes_5),
        format_minutes_1(minutes_1),
    ])


def convert_string_to_berlin(input_string):
    return format_berlin_data(*convert_timetuple_to_berlin_data(*convert_string_to_timetuple(input_string)))


verification_data = [
    ("23:59:59",
     """O
RRRR
RRRO
YYRYYRYYRYY
YYYY"""),
    ("00:00:00",
     """Y
OOOO
OOOO
OOOOOOOOOOO
OOOO"""),
    ("13:17:01",
     """O
RROO
RRRO
YYROOOOOOOO
YYOO"""),
    ("24:00:00",
     """Y
RRRR
RRRR
OOOOOOOOOOO
OOOO""")
]

for test_data in verification_data:
    input = test_data[0]
    result = convert_string_to_berlin(input)
    expected = test_data[1]
    assert result == expected, "Output for {} is: \n{} \n Is does not equal: \n{}".format(input, result, expected)
