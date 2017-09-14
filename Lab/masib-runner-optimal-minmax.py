import subprocess
from collections import defaultdict

algorithms = [ 'dfbnb']
problems = [ 'box-od']
snake_heuristics = ['none' ]
box_heuristics = ['none','shortest']
# dimentions_spreads_list = [(5, 2), (5, 3), (6, 2), (6, 3), (6, 4), (7, 2), (7, 3), (7, 4), (7, 5), (8, 3), (8, 4), (8, 5), (8, 6), (9, 4), (9, 5), (9, 6), (9, 7)] # only relevant with time aspect
dimentions_spreads_list = [(4, 2),(5, 2), (5, 3), (6, 2), (6, 3), (6, 4), (7, 3), (7, 4), (7, 5), (8, 4), (8, 5), (8, 6), (9, 5), (9, 6), (9, 7)] # only relevant with time aspect
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
                        for bx_sprd in spreads:
                            if bx_sprd < snk_sprd:
                                continue
                            for num_of_snakes in range(2,5):
                                try:
                                    count += 1
                                    print('Running algorithms:{}  problems:{} snakeh:{} boxh:{} dim:{} snakeSpread:{} boxSpread:{} num_of_snakes:{}'.format(
                                        alg, prob, snakeh, boxh, dim, snk_sprd, bx_sprd, num_of_snakes))
                                    args = ['MaSib.exe', 'alg=' + alg, 'problem=' + prob, 'snakeH=' + snakeh, 'boxH=' + boxh,
                                            'dim=' + str(dim), 'snakeSpread=' + str(snk_sprd), 'boxSpread=' + str(bx_sprd), 'timeLimit=15', 'numOfSnakes=' + str(num_of_snakes)]
                                    subprocess.call(args)
                                except Exception:
                                    print('ERROR!!ERROR!!ERROR!!ERROR!!ERROR!!ERROR!!ERROR!! CYCLE SKIPPED')
                                    pass
print('Count:{}'.format(count))