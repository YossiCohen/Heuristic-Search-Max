import subprocess
from collections import defaultdict

def choose_iter(elements, length):
    for i in range(len(elements)):
        if length == 1:
            yield (elements[i],)
        else:
            for next in choose_iter(elements[i+1:len(elements)], length-1):
                yield (elements[i],) + next
def choose(l, k):
    return list(choose_iter(l, k))


def Solve_MASIB(algorithm, problem, snake_heuristic, box_heuristic, dimension, snake_spread, box_spread,
                number_of_agents, time_limit_minutes = 120):
    lst = range(1, 2 ** dimension)
    snakes_combinations = choose(lst, number_of_agents - 1)
    print('Running MaSib Solver for {} times, later, use ExpSum.exe or read the log files to find the optimal solution'.format(len(snakes_combinations)))

    total_launces = 0
    for snakes_list in snakes_combinations:
        total_launces += 1
        print('Running MaSib - Launch number: {} of {}'.format(total_launces, len(snakes_combinations)))

        states = ['s0=0']
        snake_counter = 1
        for snake in snakes_list:
            states.append('s{}={}'.format(snake_counter, snake))
            snake_counter += 1
        try:
            print('Running algorithms:{}  problems:{} snake_heuristic:{} box_heuristic:{} dimension:{} snake_spread:{} box_spread:{} locations:{} time_limit_minutes{}'.format(
                algorithm, problem, snake_heuristic, box_heuristic, dimension, snake_spread, box_spread, states, time_limit_minutes))
            args = ['MaSib.exe', 'alg=' + algorithm, 'problem=' + problem, 'snakeH=' + snake_heuristic, 'boxH=' + box_heuristic,
                    'dim=' + str(dimension), 'snakeSpread=' + str(snake_spread), 'boxSpread=' + str(box_spread),
                    'timeLimit='+str(time_limit_minutes)] + states

            subprocess.call(args)
        except Exception:
            print('ERROR!!ERROR!!ERROR!!ERROR!!ERROR!!ERROR!!ERROR!! CYCLE SKIPPED')
            pass

algorithm = 'dfbnb'
problem = 'box-od'
snake_heuristics = 'none'
box_heuristics = 'legal'
dimension = 7
snake_spread = 2
box_spread = 3
number_of_agents = 4

Solve_MASIB(algorithm, problem, snake_heuristics, box_heuristics, dimension, snake_spread, box_spread, number_of_agents, 30)
