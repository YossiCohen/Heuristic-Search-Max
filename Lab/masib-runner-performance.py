import subprocess
from collections import defaultdict

algorithms = ['astar', 'dfbnb']
problems = ['snake', 'box', 'box-od']
snake_heuristics = ['none', 'legal', 'reachable']
box_heuristics = ['none', 'snakes-sum', 'legal', 'reachable']
# dimentions_spreads_list = [(5, 2), (5, 3), (6, 2), (6, 3), (6, 4), (7, 2), (7, 3), (7, 4), (7, 5), (8, 3), (8, 4), (8, 5), (8, 6), (9, 4), (9, 5), (9, 6), (9, 7)] # only relevant with time aspect
dimentions_spreads_list = [(5, 2), (5, 3), (6, 2), (6, 3), (6, 4), (7, 3), (7, 4), (7, 5), (8, 4), (8, 5), (8, 6), (9, 5), (9, 6), (9, 7)] # only relevant with time aspect
dims_dict = defaultdict(list)
for k, v in dimentions_spreads_list:
    dims_dict[k].append(v)

count = 0
for alg in algorithms:
    for prob in problems:
        for snakeh in snake_heuristics:
            for boxh in box_heuristics:
                for dim, spreads in dims_dict.items():
                    for snk_sprd in spreads:
                        did_snake_once = False
                        for bx_sprd in spreads:
                            #Filter: only one boxh on 'snake' problem
                            if prob == 'snake' and did_snake_once:
                                continue
                            if prob == 'snake' and boxh != 'none':
                                continue
                            if bx_sprd < snk_sprd:
                                continue
                            states = ['s0=0']
                            if prob != 'snake':
                                secondSnakeLocation = (2**dim) - 1
                                states.append('s1=' + str(secondSnakeLocation))

                            try:
                                count += 1
                                print('Running algorithms:{}  problems:{} snakeh:{} boxh:{} dim:{} snakeSpread:{} boxSpread:{} locations:{}'.format(
                                    alg, prob, snakeh, boxh, dim, snk_sprd, bx_sprd, states))
                                args = ['MaSib.exe', 'alg=' + alg, 'problem=' + prob, 'snakeH=' + snakeh, 'boxH=' + boxh, 'dim=' + str(dim), 'snakeSpread=' + str(snk_sprd), 'boxSpread=' + str(bx_sprd), 'timeLimit=15' ] + states
                                subprocess.call(args)
                            except Exception:
                                print('ERROR!!ERROR!!ERROR!!ERROR!!ERROR!!ERROR!!ERROR!! CYCLE SKIPPED')
                                pass
                            if prob == 'snake':
                                did_snake_once = True
print('Count:{}'.format(count))