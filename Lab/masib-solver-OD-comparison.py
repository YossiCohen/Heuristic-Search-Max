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

algorithms = [ 'dfbnb', 'astar']
problems = [ 'box-od']
snake_heuristics = ['none' ]
box_heuristics = ['legal', 'reachable']

# Tow agents: 6,2,2 -733-844 - 9,5,5
# Three Agents: 8,3,3
# Four Agents: 7,2,2
dimentions_spreads_list = [(6, 2), (9, 5)] # only relevant with time aspect
dims_dict = defaultdict(list)
for k, v in dimentions_spreads_list:
    dims_dict[k].append(v)

for alg in algorithms:
    for prob in problems:
        for snakeh in snake_heuristics:
            for boxh in box_heuristics:
                for dim, spreads in dims_dict.items():
                    for snk_sprd in spreads:
                        for bx_sprd in spreads:
                            if bx_sprd < snk_sprd:
                                continue
                            Solve_MASIB(alg, prob, snakeh, boxh, dim, snk_sprd, bx_sprd, 2, 30)
