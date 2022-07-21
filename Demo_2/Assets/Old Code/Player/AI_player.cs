using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_player : Player
{
    /*
    float sqore_void;
    float sqore_self;
    float sqore_another;
    float destroy_figure ;
    float sqore_sucide;


    int MAX_LEVEL = 4;

    private void Start()
    {
        sqore_void     = 3;
        sqore_self     = -1;
        sqore_another  = 5;
        destroy_figure = 100;
        sqore_sucide   = -50;
    }

    public override void step()
    {
        if (figures.Count == 0) GameObject.FindWithTag("play_session").GetComponent<Play_session>().step();
        StartCoroutine(_step());
        //alpha_beta(GameObject.FindWithTag("play_session").GetComponent<Play_session>().get_last_step_description(), 0, int.MinValue, int.MaxValue);
    }


    public int alpha_beta(Step_description step_description, int level, int alpha, int beta)
    {
        int max_min_evaluation;

        //var best_step;

        // Проверка на конец игры

        if (level >= MAX_LEVEL)
        {
            return evaluation_function(step_description);
        }

        // Ход бота
        if (level % 2 == 0)
        {
            max_min_evaluation = int.MinValue;

            for (int i = 0; i < figures.Count; i++)
            {
                if (beta < alpha) break;

                Figure i_figure = figures[i];
                if (i_figure.get_live() == false) continue;

                List<Vector3> avaible = i_figure.get_available();

                // ?
                int j_score = 0;

                // ?
                Vector3 best_avaible_i = new Vector3();

                for (int j = 0; j < avaible.Count; j++)
                {
                    if (beta < alpha) break;
                    if (max_min_evaluation >= beta) break;



                    Vector3 j_avaible = avaible[j];

                    // copy?
                    Step_description fake_step_description = (step_description);

                    fake_step_description = move_fake(fake_step_description, i_figure, j_avaible);


                    int _min_max = alpha_beta(fake_step_description, level + 1, alpha, beta);

                    // Если мы в корне дерева, запоминаем наилучший ход
                    if (level == 0 && _min_max > max_min_evaluation)
                    {
                        //best = pair<Point, Point>(avaible[i].first, avaible[i].second[j]);
                    }

                    max_min_evaluation = Math.Max(max_min_evaluation, _min_max);

                    alpha = Math.Max(alpha, max_min_evaluation);




                }
            }

        }
        // Ходит человек
        else
        {
            max_min_evaluation = int.MaxValue;

            for (int i = 0; i < figures.Count; i++)
            {
                if (beta < alpha) break;

                Figure i_figure = figures[i];
                if (i_figure.get_live() == false) continue;

                List<Vector3> avaible = i_figure.get_available();

                // ?
                int j_score = 0;

                // ?
                Vector3 best_avaible_i = new Vector3();

                for (int j = 0; j < avaible.Count; j++)
                {
                    if (beta < alpha) break;
                    if (max_min_evaluation <= alpha) break;



                    Vector3 j_avaible = avaible[j];


                    Step_description fake_step_description = step_description;
                    move_fake(fake_step_description, i_figure, j_avaible);


                    int _min_max = alpha_beta(fake_step_description, level + 1, alpha, beta);

                    // Если мы в корне дерева, запоминаем наилучший ход
                    if (level == 0 && _min_max > max_min_evaluation)
                    {
                        //best = pair<Point, Point>(avaible[i].first, avaible[i].second[j]);
                    }

                    max_min_evaluation = Math.Min(max_min_evaluation, _min_max);

                    beta = Math.Min(alpha, max_min_evaluation);

                }
            }
        }


        if (level == 0) 
        {
            Debug.Log("LEVEL 0"); 
        }

        return max_min_evaluation;
    }


    public int evaluation_function(Step_description step_Description) 
    {

        return 1;
    }

    public Step_description move_fake(Step_description step_Description, Figure figure, Vector3 step) 
    {
        ///////////////////////////////////////
        ///


        figure.figure_down();



        return step_Description;
    }




    private float calculate_sqore(Vector3 avaible, Figure figure, Vector3 start_position)
    {
        float sqore = 0;
        List<Vector3> way = figure.calculate_way(avaible, start_position);

        Board _board = GameObject.FindWithTag("Board").GetComponent<Board>();

        for (int i = 0; i < way.Count; i++)
        {
            Cell _cell = _board.get_cell(way[i].x, way[i].z);

            cell_description _status = _cell.get_status();

            if (figure.transform.localPosition.x == way[i].x && figure.transform.localPosition.z == way[i].z) continue;


            if (figure.name.Remove(4) == "Pawn" && _cell.get_figure_in_cell() && (_cell.get_who_in_cell().number_player != figure.number_player) && (_status.status != "dark"))
            {
                sqore += destroy_figure;
            }

            if (_status.status == "void")
            {
                // Пустая клетка 
                sqore += sqore_void;
            }
            else if (_status.number_player == player_number)
            {
                // Наступить на свою клетку
                sqore += sqore_self;
            }
            else if (_status.number_player != player_number)
            {
                // Перекрасить чужую клетку 
                sqore += sqore_another;
            }

            if ((i == way.Count - 1) && sucide_figure(way[i], _board)) 
            {
                sqore += sqore_sucide;
            }
        }

        //Debug.Log("Если я схожу " + figure.name + " на " + avaible.x + " , " + avaible.z + " то получу за это условных: " + sqore);
        return sqore;
    }

    IEnumerator _step()
    {
        yield return new WaitForSeconds(0f);

        Figure best_figure = figures[0];
        Vector3 best_avaible = best_figure.get_available()[0];
        int best_scqore = 0;
        int best_scqore_i = 0;

        for (int i = 0; i < figures.Count; i++)
        {
            //Debug.Log("Я сейчас проверяю фигиру: " + figures[i].name);

            Figure i_figure = figures[i];
            if (i_figure.get_live() == false) continue;
            
            List<Vector3> avaible = i_figure.get_available();


            int j_score = 0;

            Vector3 best_avaible_i = new Vector3().normalized;

            for (int j = 0; j < avaible.Count; j++)
            {
                Vector3 j_avaible = avaible[j];
                j_score = (int) calculate_sqore(j_avaible, i_figure, i_figure.transform.localPosition);
                

                if (j_score > best_scqore_i)
                {
                    best_scqore_i = j_score;
                    best_avaible_i = j_avaible;
                }
            }

            if (best_scqore_i > best_scqore)
            {
                best_scqore = best_scqore_i;
                best_avaible = best_avaible_i;
                best_figure = figures[i];
            }
        }

        //Debug.Log("Как по мне лучший ход это идти фигурой: " + best_figure.name + " на клетку: (" +  best_avaible.x + ", " + best_avaible.z + ")");


        best_figure.move_figure(best_avaible);

    }

    bool sucide_figure(Vector3 _cell_main, Board _Board) 
    {
        bool sucide = false;

        for (float i = (_cell_main.x - 1f);  i <= (_cell_main.x + 1f); i++)
        {
            if (i < 0 || i > _Board.get_lenght() - 1) { continue; }

            for (float j = (_cell_main.z - 1f); j <= (_cell_main.z + 1f); j++)
            {
                if (j < 0 || j > _Board.get_width() - 1) { continue; }

                if (i == _cell_main.x && j == _cell_main.z) { continue; }
               
                if (_Board.get_cell(i, j).get_who_in_cell() != null &&
                    _Board.get_cell(i, j).get_who_in_cell().name.Remove(4) == "Pawn" &&
                    _Board.get_cell(i, j).get_who_in_cell().number_player != player_number &&
                    _Board.get_cell(i, j).get_status().status != "dark") 
                {
                    return true;
                }
            }
        }


        return sucide;
    }
    */
}
