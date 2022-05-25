import React, {useEffect} from 'react';
import {Link} from "react-router-dom";
import {useDispatch, useSelector} from "react-redux";
import {RootState} from "../../../redux/store";
import {todosActions} from "../../../redux/todos/actions";
import {Switch, Table} from "antd";
import {ColumnsType} from "antd/es/table";
import {Todo} from "../../../gql/modules/todos/todos.types";
import {ButtonsVUR} from "../../../components/ButtonsVUD/ButtonsVUR";
import Title from 'antd/lib/typography/Title';

export const ToDosIndex = () => {
    const dispatch = useDispatch();
    const todos = useSelector((s: RootState) => s.todos.todos)

    useEffect(() => {
        if (todos.length === 0)
            dispatch(todosActions.getTodos());
    }, [])

    const columns: ColumnsType<Todo> = [
        {
            title: 'Is complete',
            dataIndex: 'isComplete',
            key: 'isComplete',
            width: '110px',
            render: (text, todo) => (
                <Switch size="small" defaultChecked/>
            ),
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Actions',
            dataIndex: 'actions',
            key: 'actions',
            width: '130px',
            render: (text, todo) => (
                <ButtonsVUR updateUrl={`update/${todo.id}`} /*onRemove={() => onRemove(grade?.id)}*//>
            ),
        },
    ];

    return (
        <div>
            <Title level={2}>Todos</Title>
            <Link to={'/todos/create'}>create</Link>
            <Table
                rowKey={'id'}
                dataSource={todos}
                columns={columns}
                pagination={false}
            />

        </div>
    );
};