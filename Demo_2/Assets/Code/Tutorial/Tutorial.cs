using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static bool _tutorual;

    // PTF - position tutorial figure - словарь позиций фигур
    // SDT - step description tutorial - словарь статуса игры
    // SC  - status cell - статус клетки
    // SB  - scale board - размер доски 

    public Play_session_settings pss;
    public UI ui;
    public List<Player_description> playersDiscription = new List<Player_description>();
    Number_Tutorial now_number_Tutorial;
    public GameObject nextTutorial;
    public GameObject backTutorial;

    #region Обучение фигур

    #region Статусы клеток

    cell_description SC_paint_self    =  new cell_description("paint", 1);
    cell_description SC_dark_self     =  new cell_description("dark", 1);
    cell_description SC_paint_another =  new cell_description("paint", 2);
    cell_description SC_dark_another  =  new cell_description("dark", 2);
    cell_description SC_finish        =  new cell_description("finished", 10);

    #endregion

    #region Пешка


    Step_description SDT_1 = new Step_description() { };

    int SB_1 = 7;

    Dictionary<Vector3, string> PTF_1_1 = new Dictionary<Vector3, string>
    {
        {new Vector3(3f, 0f, 3f), "Pawn"},
        {new Vector3(2f, 0f, 2f), "Pawn"},
        {new Vector3(2f, 0f, 4f), "Pawn"},
    };

    Dictionary<Vector3, string> PTF_1_2 = new Dictionary<Vector3, string>
    {
        {new Vector3(2f, 0f, 2f), "Pawn"},
        {new Vector3(2f, 0f, 4f), "Pawn"},
        {new Vector3(2f, 0f, 5f), "Castle"},
        {new Vector3(1f, 0f, 4f), "Queen"},
        {new Vector3(2f, 0f, 1f), "King"},
        {new Vector3(1f, 0f, 2f), "Bishop"},
    };

    void fill_SDT_1()
    {
        SDT_1 = fill_void_step_description(SB_1);

        SDT_1 = fill_figure_step_description(SDT_1, PTF_1_1, SC_paint_self, Corner.Down_left, SB_1);
        SDT_1 = fill_figure_step_description(SDT_1, PTF_1_2, SC_paint_another, Corner.Up_right, SB_1);

        SDT_1.massive_cell[0, 4].cell = SC_dark_self;
        SDT_1.massive_cell[0, 5].cell = SC_dark_self;
        SDT_1.massive_cell[0, 6].cell = SC_dark_self;
        SDT_1.massive_cell[1, 4].cell = SC_dark_self;
        SDT_1.massive_cell[1, 5].cell = SC_dark_self;
        SDT_1.massive_cell[1, 6].cell = SC_dark_self;
        SDT_1.massive_cell[2, 4].cell = SC_dark_self;
        SDT_1.massive_cell[2, 5].cell = SC_dark_self;
        SDT_1.massive_cell[2, 6].cell = SC_dark_self;

        SDT_1.massive_cell[4, 4].cell = SC_dark_another;
        SDT_1.massive_cell[4, 5].cell = SC_dark_another;
        SDT_1.massive_cell[4, 6].cell = SC_dark_another;
        SDT_1.massive_cell[5, 4].cell = SC_dark_another;
        SDT_1.massive_cell[5, 5].cell = SC_dark_another;
        SDT_1.massive_cell[5, 6].cell = SC_dark_another;
        SDT_1.massive_cell[6, 4].cell = SC_dark_another;
        SDT_1.massive_cell[6, 5].cell = SC_dark_another;
        SDT_1.massive_cell[6, 6].cell = SC_dark_another;

        SDT_1.massive_cell[0, 1].cell = SC_paint_self;
        SDT_1.massive_cell[0, 2].cell = SC_paint_self;
        SDT_1.massive_cell[1, 0].cell = SC_paint_self;
        SDT_1.massive_cell[1, 1].cell = SC_paint_self;
        SDT_1.massive_cell[2, 0].cell = SC_paint_self;
        SDT_1.massive_cell[1, 2].cell = SC_paint_self;
        SDT_1.massive_cell[2, 1].cell = SC_paint_self;

        SDT_1.massive_cell[4, 0].cell = SC_paint_another;
        SDT_1.massive_cell[4, 1].cell = SC_paint_another;
        SDT_1.massive_cell[4, 2].cell = SC_paint_another;
        SDT_1.massive_cell[5, 0].cell = SC_paint_another;
        SDT_1.massive_cell[5, 1].cell = SC_paint_another;
        SDT_1.massive_cell[5, 2].cell = SC_paint_another;
        SDT_1.massive_cell[6, 1].cell = SC_paint_another;
        SDT_1.massive_cell[6, 2].cell = SC_paint_another;

    }

    #endregion

    #region Король

    Step_description SDT_2 = new Step_description() { };

    int SB_2 = 7;
    Dictionary<Vector3, string> PTF_2_1 = new Dictionary<Vector3, string>
    {
        {new Vector3(3f, 0f, 3f), "King"},
        //{new Vector3(2f, 0f, 2f), "Pawn"},
        //{new Vector3(2f, 0f, 4f), "Pawn"},
    };

    Dictionary<Vector3, string> PTF_2_2 = new Dictionary<Vector3, string>
    {
        //{new Vector3(3f, 0f, 3f), "King"},
        {new Vector3(1f, 0f, 2f), "Pawn"},
        {new Vector3(1f, 0f, 3f), "Pawn"},
        {new Vector3(1f, 0f, 4f), "Pawn"},
    };


    void fill_SDT_2()
    {
        SDT_2 = fill_void_step_description(SB_2);

        SDT_2 = fill_figure_step_description(SDT_2, PTF_2_1, SC_paint_self, Corner.Down_left, SB_2);
        SDT_2 = fill_figure_step_description(SDT_2, PTF_2_2, SC_paint_another, Corner.Up_right, SB_2);

        SDT_2.massive_cell[0, 4].cell = SC_dark_self;
        SDT_2.massive_cell[0, 5].cell = SC_dark_self;
        SDT_2.massive_cell[0, 6].cell = SC_dark_self;
        SDT_2.massive_cell[1, 4].cell = SC_dark_self;
        SDT_2.massive_cell[1, 5].cell = SC_dark_self;
        SDT_2.massive_cell[1, 6].cell = SC_dark_self;
        SDT_2.massive_cell[2, 4].cell = SC_dark_self;
        SDT_2.massive_cell[2, 5].cell = SC_dark_self;
        SDT_2.massive_cell[2, 6].cell = SC_dark_self;

        SDT_2.massive_cell[4, 4].cell = SC_dark_another;
        SDT_2.massive_cell[4, 5].cell = SC_dark_another;
        SDT_2.massive_cell[4, 6].cell = SC_dark_another;
        SDT_2.massive_cell[5, 4].cell = SC_dark_another;
        SDT_2.massive_cell[5, 5].cell = SC_dark_another;
        SDT_2.massive_cell[5, 6].cell = SC_dark_another;
        SDT_2.massive_cell[6, 4].cell = SC_dark_another;
        SDT_2.massive_cell[6, 5].cell = SC_dark_another;
        SDT_2.massive_cell[6, 6].cell = SC_dark_another;

        SDT_2.massive_cell[0, 1].cell = SC_paint_self;
        SDT_2.massive_cell[0, 2].cell = SC_paint_self;
        SDT_2.massive_cell[1, 0].cell = SC_paint_self;
        SDT_2.massive_cell[1, 1].cell = SC_paint_self;
        SDT_2.massive_cell[2, 0].cell = SC_paint_self;
        SDT_2.massive_cell[1, 2].cell = SC_paint_self;
        SDT_2.massive_cell[2, 1].cell = SC_paint_self;
        SDT_2.massive_cell[2, 2].cell = SC_paint_self;

        SDT_2.massive_cell[4, 0].cell = SC_paint_another;
        SDT_2.massive_cell[4, 1].cell = SC_paint_another;
        SDT_2.massive_cell[4, 2].cell = SC_paint_another;
        SDT_2.massive_cell[5, 0].cell = SC_paint_another;
        SDT_2.massive_cell[5, 1].cell = SC_paint_another;
        SDT_2.massive_cell[5, 2].cell = SC_paint_another;
        SDT_2.massive_cell[6, 1].cell = SC_paint_another;
        SDT_2.massive_cell[6, 2].cell = SC_paint_another;
        //SDT_2.massive_cell[6, 6].cell = SC_paint_another;
    }

    #endregion

    #region Ладья - Castle 

    Step_description SDT_3 = new Step_description() { };

    int SB_3 = 7;
    Dictionary<Vector3, string> PTF_3_1 = new Dictionary<Vector3, string>
    {
        {new Vector3(3f, 0f, 3f), "Castle"},
        {new Vector3(1f, 0f, 5f), "Castle"},
        {new Vector3(1f, 0f, 1f), "Castle"},
    };

    Dictionary<Vector3, string> PTF_3_2 = new Dictionary<Vector3, string>
    { 
        {new Vector3(1f, 0f, 1f), "Castle"},
        {new Vector3(1f, 0f, 5f), "Castle"},
    };


    void fill_SDT_3()
    {
        SDT_3 = fill_void_step_description(SB_3);

        SDT_3 = fill_figure_step_description(SDT_3, PTF_3_1, SC_paint_self, Corner.Down_left, SB_3);
        SDT_3 = fill_figure_step_description(SDT_3, PTF_3_2, SC_paint_another, Corner.Up_right, SB_3);

        SDT_3.massive_cell[0, 4].cell = SC_dark_self;
        SDT_3.massive_cell[0, 5].cell = SC_dark_self;
        SDT_3.massive_cell[0, 6].cell = SC_dark_self;
        SDT_3.massive_cell[1, 4].cell = SC_dark_self;
        SDT_3.massive_cell[1, 5].cell = SC_dark_self;
        SDT_3.massive_cell[1, 6].cell = SC_dark_self;
        SDT_3.massive_cell[2, 4].cell = SC_dark_self;
        SDT_3.massive_cell[2, 5].cell = SC_dark_self;
        SDT_3.massive_cell[2, 6].cell = SC_dark_self;

        SDT_3.massive_cell[4, 4].cell = SC_dark_another;
        SDT_3.massive_cell[4, 5].cell = SC_dark_another;
        SDT_3.massive_cell[4, 6].cell = SC_dark_another;
        SDT_3.massive_cell[5, 4].cell = SC_dark_another;
        SDT_3.massive_cell[5, 5].cell = SC_dark_another;
        SDT_3.massive_cell[5, 6].cell = SC_dark_another;
        SDT_3.massive_cell[6, 4].cell = SC_dark_another;
        SDT_3.massive_cell[6, 5].cell = SC_dark_another;
        SDT_3.massive_cell[6, 6].cell = SC_dark_another;

        SDT_3.massive_cell[0, 1].cell = SC_paint_self;
        SDT_3.massive_cell[0, 2].cell = SC_paint_self;
        SDT_3.massive_cell[1, 0].cell = SC_paint_self;
        SDT_3.massive_cell[1, 1].cell = SC_paint_self;
        SDT_3.massive_cell[2, 0].cell = SC_paint_self;
        SDT_3.massive_cell[1, 2].cell = SC_paint_self;
        SDT_3.massive_cell[2, 1].cell = SC_paint_self;
        SDT_3.massive_cell[2, 2].cell = SC_paint_self;

        SDT_3.massive_cell[4, 0].cell = SC_paint_another;
        SDT_3.massive_cell[4, 1].cell = SC_paint_another;
        SDT_3.massive_cell[4, 2].cell = SC_paint_another;
        SDT_3.massive_cell[5, 0].cell = SC_paint_another;
        SDT_3.massive_cell[5, 1].cell = SC_paint_another;
        SDT_3.massive_cell[5, 2].cell = SC_paint_another;
        SDT_3.massive_cell[6, 1].cell = SC_paint_another;
        SDT_3.massive_cell[6, 2].cell = SC_paint_another;
        //SDT_2.massive_cell[6, 6].cell = SC_paint_another;
    }

    #endregion

    #region Слон - Bishop

    Step_description SDT_4 = new Step_description() { };

    int SB_4 = 7;
    Dictionary<Vector3, string> PTF_4_1 = new Dictionary<Vector3, string>
    {
        //{new Vector3(3f, 0f, 3f), "Bishop"},
        {new Vector3(1f, 0f, 5f), "Bishop"},
        {new Vector3(1f, 0f, 1f), "Bishop"},
    };

    Dictionary<Vector3, string> PTF_4_2 = new Dictionary<Vector3, string>
    {
        {new Vector3(1f, 0f, 1f), "King"},
        {new Vector3(1f, 0f, 5f), "Queen"},
    };


    void fill_SDT_4()
    {
        SDT_4 = fill_void_step_description(SB_4);

        SDT_4 = fill_figure_step_description(SDT_4, PTF_4_1, SC_paint_self, Corner.Down_left, SB_4);
        SDT_4 = fill_figure_step_description(SDT_4, PTF_4_2, SC_paint_another, Corner.Up_right, SB_4);

        SDT_4.massive_cell[0, 4].cell = SC_dark_self;
        SDT_4.massive_cell[0, 5].cell = SC_dark_self;
        SDT_4.massive_cell[0, 6].cell = SC_dark_self;
        SDT_4.massive_cell[1, 4].cell = SC_dark_self;
        SDT_4.massive_cell[1, 5].cell = SC_dark_self;
        SDT_4.massive_cell[1, 6].cell = SC_dark_self;
        SDT_4.massive_cell[2, 4].cell = SC_dark_self;
        SDT_4.massive_cell[2, 5].cell = SC_dark_self;
        SDT_4.massive_cell[2, 6].cell = SC_dark_self;

        SDT_4.massive_cell[4, 4].cell = SC_dark_another;
        SDT_4.massive_cell[4, 5].cell = SC_dark_another;
        SDT_4.massive_cell[4, 6].cell = SC_dark_another;
        SDT_4.massive_cell[5, 4].cell = SC_dark_another;
        SDT_4.massive_cell[5, 5].cell = SC_dark_another;
        SDT_4.massive_cell[5, 6].cell = SC_dark_another;
        SDT_4.massive_cell[6, 4].cell = SC_dark_another;
        SDT_4.massive_cell[6, 5].cell = SC_dark_another;
        SDT_4.massive_cell[6, 6].cell = SC_dark_another;

        SDT_4.massive_cell[0, 1].cell = SC_paint_self;
        SDT_4.massive_cell[0, 2].cell = SC_paint_self;
        SDT_4.massive_cell[1, 0].cell = SC_paint_self;
        SDT_4.massive_cell[1, 1].cell = SC_paint_self;
        SDT_4.massive_cell[2, 0].cell = SC_paint_self;
        SDT_4.massive_cell[1, 2].cell = SC_paint_self;
        SDT_4.massive_cell[2, 1].cell = SC_paint_self;
        SDT_4.massive_cell[2, 2].cell = SC_paint_self;
    
        SDT_4.massive_cell[4, 0].cell = SC_paint_another;
        SDT_4.massive_cell[4, 1].cell = SC_paint_another;
        SDT_4.massive_cell[4, 2].cell = SC_paint_another;
        SDT_4.massive_cell[5, 0].cell = SC_paint_another;
        SDT_4.massive_cell[5, 1].cell = SC_paint_another;
        SDT_4.massive_cell[5, 2].cell = SC_paint_another;
        SDT_4.massive_cell[6, 1].cell = SC_paint_another;
        SDT_4.massive_cell[6, 2].cell = SC_paint_another;
        //SD4T_2.massive_cell[6, 6].cell = SC_paint_another;
    }

    #endregion

    #region Королева

    Step_description SDT_5 = new Step_description() { };

    int SB_5 = 7;
    Dictionary<Vector3, string> PTF_5_1 = new Dictionary<Vector3, string>
    {
        {new Vector3(3f, 0f, 3f), "Queen"},
        //{new Vector3(2f, 0f, 2f), "Pawn"},
        //{new Vector3(2f, 0f, 4f), "Pawn"},
    };

    Dictionary<Vector3, string> PTF_5_2 = new Dictionary<Vector3, string>
    {
        //{new Vector3(3f, 0f, 3f), "King"},
        {new Vector3(1f, 0f, 2f), "Pawn"},
        {new Vector3(1f, 0f, 3f), "Pawn"},
        {new Vector3(1f, 0f, 4f), "Pawn"},
    };


    void fill_SDT_5()
    {
        SDT_5 = fill_void_step_description(SB_5);

        SDT_5 = fill_figure_step_description(SDT_5, PTF_5_1, SC_paint_self, Corner.Down_left, SB_5);
        SDT_5 = fill_figure_step_description(SDT_5, PTF_5_2, SC_paint_another, Corner.Up_right, SB_5);

        SDT_5.massive_cell[0, 4].cell = SC_dark_self;
        SDT_5.massive_cell[0, 5].cell = SC_dark_self;
        SDT_5.massive_cell[0, 6].cell = SC_dark_self;
        SDT_5.massive_cell[1, 4].cell = SC_dark_self;
        SDT_5.massive_cell[1, 5].cell = SC_dark_self;
        SDT_5.massive_cell[1, 6].cell = SC_dark_self;
        SDT_5.massive_cell[2, 4].cell = SC_dark_self;
        SDT_5.massive_cell[2, 5].cell = SC_dark_self;
        SDT_5.massive_cell[2, 6].cell = SC_dark_self;

        SDT_5.massive_cell[4, 4].cell = SC_dark_another;
        SDT_5.massive_cell[4, 5].cell = SC_dark_another;
        SDT_5.massive_cell[4, 6].cell = SC_dark_another;
        SDT_5.massive_cell[5, 4].cell = SC_dark_another;
        SDT_5.massive_cell[5, 5].cell = SC_dark_another;
        SDT_5.massive_cell[5, 6].cell = SC_dark_another;
        SDT_5.massive_cell[6, 4].cell = SC_dark_another;
        SDT_5.massive_cell[6, 5].cell = SC_dark_another;
        SDT_5.massive_cell[6, 6].cell = SC_dark_another;
    
        SDT_5.massive_cell[0, 1].cell = SC_paint_self;
        SDT_5.massive_cell[0, 2].cell = SC_paint_self;
        SDT_5.massive_cell[1, 0].cell = SC_paint_self;
        SDT_5.massive_cell[1, 1].cell = SC_paint_self;
        SDT_5.massive_cell[2, 0].cell = SC_paint_self;
        SDT_5.massive_cell[1, 2].cell = SC_paint_self;
        SDT_5.massive_cell[2, 1].cell = SC_paint_self;
        SDT_5.massive_cell[2, 2].cell = SC_paint_self;
        
        SDT_5.massive_cell[4, 0].cell = SC_paint_another;
        SDT_5.massive_cell[4, 1].cell = SC_paint_another;
        SDT_5.massive_cell[4, 2].cell = SC_paint_another;
        SDT_5.massive_cell[5, 0].cell = SC_paint_another;
        SDT_5.massive_cell[5, 1].cell = SC_paint_another;
        SDT_5.massive_cell[5, 2].cell = SC_paint_another;
        SDT_5.massive_cell[6, 1].cell = SC_paint_another;
        SDT_5.massive_cell[6, 2].cell = SC_paint_another;
        //SDT_2.massive_cell[6, 6].cell = SC_paint_another;
    }

    #endregion

    #region Конь

    Step_description SDT_6 = new Step_description() { };

    int SB_6 = 7;
    Dictionary<Vector3, string> PTF_6_1 = new Dictionary<Vector3, string>
    {
        {new Vector3(3f, 0f, 3f), "Horse"},
        //{new Vector3(2f, 0f, 2f), "Pawn"},
        //{new Vector3(2f, 0f, 4f), "Pawn"},
    };

    Dictionary<Vector3, string> PTF_6_2 = new Dictionary<Vector3, string>
    {
        //{new Vector3(3f, 0f, 3f), "King"},
        {new Vector3(2f, 0f, 2f), "Pawn"},
        {new Vector3(2f, 0f, 3f), "Pawn"},
        {new Vector3(2f, 0f, 4f), "Pawn"},
    };


    void fill_SDT_6()
    {
        SDT_6 = fill_void_step_description(SB_6);

        SDT_6 = fill_figure_step_description(SDT_6, PTF_6_1, SC_paint_self, Corner.Down_left, SB_6);
        SDT_6 = fill_figure_step_description(SDT_6, PTF_6_2, SC_paint_another, Corner.Up_right, SB_6);

        SDT_6.massive_cell[0, 4].cell = SC_dark_self;
        SDT_6.massive_cell[0, 5].cell = SC_dark_self;
        SDT_6.massive_cell[0, 6].cell = SC_dark_self;
        SDT_6.massive_cell[1, 4].cell = SC_dark_self;
        SDT_6.massive_cell[1, 5].cell = SC_dark_self;
        SDT_6.massive_cell[1, 6].cell = SC_dark_self;
        SDT_6.massive_cell[2, 4].cell = SC_dark_self;
        SDT_6.massive_cell[2, 5].cell = SC_dark_self;
        SDT_6.massive_cell[2, 6].cell = SC_dark_self;

        SDT_6.massive_cell[4, 4].cell = SC_dark_another;
        SDT_6.massive_cell[4, 5].cell = SC_dark_another;
        SDT_6.massive_cell[4, 6].cell = SC_dark_another;
        SDT_6.massive_cell[5, 4].cell = SC_dark_another;
        SDT_6.massive_cell[5, 5].cell = SC_dark_another;
        SDT_6.massive_cell[5, 6].cell = SC_dark_another;
        SDT_6.massive_cell[6, 4].cell = SC_dark_another;
        SDT_6.massive_cell[6, 5].cell = SC_dark_another;
        SDT_6.massive_cell[6, 6].cell = SC_dark_another;

        SDT_6.massive_cell[0, 1].cell = SC_paint_self;
        SDT_6.massive_cell[0, 2].cell = SC_paint_self;
        SDT_6.massive_cell[1, 0].cell = SC_paint_self;
        SDT_6.massive_cell[1, 1].cell = SC_paint_self;
        SDT_6.massive_cell[2, 0].cell = SC_paint_self;
        SDT_6.massive_cell[1, 2].cell = SC_paint_self;
        SDT_6.massive_cell[2, 1].cell = SC_paint_self;
        SDT_6.massive_cell[2, 2].cell = SC_paint_self;

        SDT_6.massive_cell[4, 0].cell = SC_paint_another;
        SDT_6.massive_cell[4, 1].cell = SC_paint_another;
        SDT_6.massive_cell[4, 2].cell = SC_paint_another;
        SDT_6.massive_cell[5, 0].cell = SC_paint_another;
        SDT_6.massive_cell[5, 1].cell = SC_paint_another;
        SDT_6.massive_cell[5, 2].cell = SC_paint_another;
        SDT_6.massive_cell[6, 1].cell = SC_paint_another;
        SDT_6.massive_cell[6, 2].cell = SC_paint_another;
        //SD_2.massive_cell[6, 6].cell = SC_paint_another;
    }

    #endregion

    #endregion






    private void Awake()
    {
        now_number_Tutorial = Number_Tutorial.Tutorial_1;
    }

    public void start_tutorial_1()
    {
        now_number_Tutorial = Number_Tutorial.Tutorial_1;

        fill_SDT_1();

        clear_players();
        set_scale_board(SB_1);
        set_camera_swap(false);

        Player_description player1 = new Player_description(PTF_1_1, Color.Red, "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(PTF_1_2, Color.Blue, "HotSeat", 2, "Player2", Corner.Up_right);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);

        set_player_description();

        pss.create_play_session(true);

        StartCoroutine(set_status_game(SDT_1));
    }


    public void start_tutorial_2()
    {
        now_number_Tutorial = Number_Tutorial.Tutorial_2;

        fill_SDT_2();

        clear_players();
        set_scale_board(SB_2);
        set_camera_swap(false);


        Player_description player1 = new Player_description(PTF_2_1, Color.Red, "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(PTF_2_2, Color.Blue, "HotSeat", 2, "Player2", Corner.Up_right);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);

        set_player_description();

        pss.create_play_session(true);

        StartCoroutine(set_status_game(SDT_2));
    }

    public void start_tutorial_3()
    {
        now_number_Tutorial = Number_Tutorial.Tutorial_3;

        fill_SDT_3();

        clear_players();
        set_scale_board(SB_3);
        set_camera_swap(false);


        Player_description player1 = new Player_description(PTF_3_1, Color.Red, "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(PTF_3_2, Color.Blue, "HotSeat", 2, "Player2", Corner.Up_right);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);

        set_player_description();

        pss.create_play_session(true);

        StartCoroutine(set_status_game(SDT_3));
    }

    public void start_tutorial_4()
    {
        now_number_Tutorial = Number_Tutorial.Tutorial_4;

        fill_SDT_4();

        clear_players();
        set_scale_board(SB_4);
        set_camera_swap(false);


        Player_description player1 = new Player_description(PTF_4_1, Color.Red, "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(PTF_4_2, Color.Blue, "HotSeat", 2, "Player2", Corner.Up_right);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);

        set_player_description();

        pss.create_play_session(true);

        StartCoroutine(set_status_game(SDT_4));
    }

    public void start_tutorial_5()
    {
        now_number_Tutorial = Number_Tutorial.Tutorial_5;

        fill_SDT_5();

        clear_players();
        set_scale_board(SB_5);
        set_camera_swap(false);


        Player_description player1 = new Player_description(PTF_5_1, Color.Red, "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(PTF_5_2, Color.Blue, "HotSeat", 2, "Player2", Corner.Up_right);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);

        set_player_description();

        pss.create_play_session(true);

        StartCoroutine(set_status_game(SDT_5));
    }

    public void start_tutorial_6()
    {
        now_number_Tutorial = Number_Tutorial.Tutorial_6;

        fill_SDT_6();

        clear_players();
        set_scale_board(SB_6);
        set_camera_swap(false);


        Player_description player1 = new Player_description(PTF_6_1, Color.Red, "HotSeat", 1, "Player1", Corner.Down_left);
        Player_description player2 = new Player_description(PTF_6_2, Color.Blue, "HotSeat", 2, "Player2", Corner.Up_right);

        playersDiscription.Add(player1);
        playersDiscription.Add(player2);

        set_player_description();

        pss.create_play_session(true);

        StartCoroutine(set_status_game(SDT_6));
    }

    public void set_player_description()
    {
        pss.clear_player_description();

        pss.set_players_discription(playersDiscription);
    }

    public void set_scale_board(int scale)
    {
        pss.set_lenght(scale);
        pss.set_width(scale);
    }


    public void set_camera_swap(bool cameraSwap)
    {
        pss.set_camera_swap(cameraSwap);
    }

    public void clear_players()
    {
        playersDiscription.Clear();
        pss.clear_player_description();
    }

    public void next_tutorial() 
    {
        int enumCount = Enum.GetNames(typeof(Number_Tutorial)).Length;

        // Если сейчас последний туторил, то включается игра с ботом
        if ((int)now_number_Tutorial >= enumCount) 
        {
            ui.DefaultSettingPlayerVSAI();
            ui.StartGame();

            return;
        }

        now_number_Tutorial++;
        hide_seek_button();
        start_tutorial();
    }

    public void back_tutorual()
    {
        int enumCount = Enum.GetNames(typeof(Number_Tutorial)).Length;

        // Если сейчас первый туторил, то дальше некуда листать
        // такого быть не должно, но на всяйкий случай будет тут проверка
        if ((int)now_number_Tutorial == 1)
        {
            return;
        }

        now_number_Tutorial--;
        hide_seek_button();
        start_tutorial();
    }

    public Step_description fill_void_step_description(int scale_board) 
    {
        Step_description step_Description = new Step_description();

        step_Description.blocked_figure = new Dictionary<Figure, int>();

        step_Description.massive_cell = new Сostil_cell[scale_board, scale_board];

        step_Description.massive_cell[0, 0].who_in_cell = null;
        step_Description.massive_cell[0, 0].cell = new cell_description("void", 0);

        for (int i = 0; i < scale_board; i++)
        {
            for (int j = 0; j < scale_board; j++)
            {
                step_Description.massive_cell[i, j].who_in_cell = null;
                step_Description.massive_cell[i, j].cell = new cell_description("void", 0);
            }
        }

        return step_Description;
    }

    public Step_description fill_figure_step_description(Step_description SDT, Dictionary<Vector3, string> PTF, cell_description cell, Corner corner = Corner.Down_left, int width = 9) 
    {
        foreach (var item in PTF)
        {
            Vector3 vect = convert_corner(item.Key, corner, width);
            SDT.massive_cell[(int)vect.x, (int)vect.z].cell = cell;
        }

        return SDT;
    }


    public void hide_seek_button() 
    {
        int enumCount = Enum.GetNames(typeof(Number_Tutorial)).Length;

        if ((int) now_number_Tutorial == enumCount) 
        {
            nextTutorial.SetActive(false);
        }
        else 
        {
            nextTutorial.SetActive(true);
        }

        if ((int) now_number_Tutorial == 1) 
        {
            backTutorial.SetActive(false);
        }
        else 
        {
            backTutorial.SetActive(true);
        }
    }

    public void start_tutorial() 
    {
        switch (now_number_Tutorial)
        {
            case Number_Tutorial.Tutorial_1:
                start_tutorial_1();
                break;
            case Number_Tutorial.Tutorial_2:
                start_tutorial_2();
                break;
            case Number_Tutorial.Tutorial_3:
                start_tutorial_3();
                break;
            case Number_Tutorial.Tutorial_4:
                start_tutorial_4();
                break;
            case Number_Tutorial.Tutorial_5:
                start_tutorial_5();
                break;
            case Number_Tutorial.Tutorial_6:
                start_tutorial_6();
                break;
            default:
                Debug.LogWarning("А что это за туторил а я не понимать (Кароче в свич добавьте туториал блин)");
                break;
        }


        hide_seek_button();
    }

    public enum Number_Tutorial
    {
        Tutorial_1 = 1,
        Tutorial_2 = 2,
        Tutorial_3 = 3,
        Tutorial_4 = 4,
        Tutorial_5 = 5,
        Tutorial_6 = 6,
    }

    public IEnumerator set_status_game(Step_description _step_description)
    {
        yield return new WaitForSeconds(1f);
        pss.set_status_game(_step_description);
        GameObject.FindWithTag("play_session").GetComponent<Play_session>().find_all_cell_down();
    }



    public Vector3 convert_corner(Vector3 convert_position, Corner corner = Corner.Down_left, int width = 9) // corner - угол
    {
        // Возвращяет конвертированные координаты, в зависимости от угла расположения на доске
        // 1 - лево-нижний угол
        // 2 - право-нижний угол
        // 3 - право-верхний угол
        // 4 - лево-верхний угол

        switch (corner)
        {
            case Corner.Down_left:
                // Ничего не меняем
                break;

            case Corner.Down_right:
                convert_position.x = width - 1 - convert_position.x;
                break;

            case Corner.Up_right:
                convert_position.x = width - 1 - convert_position.x;
                convert_position.z = width - 1 - convert_position.z;
                break;

            case Corner.Up_left:
                convert_position.z = width - 1 - convert_position.z;
                break;

            default:
                // Ничего не меняем
                break;
        }

        return convert_position;
    }
}
