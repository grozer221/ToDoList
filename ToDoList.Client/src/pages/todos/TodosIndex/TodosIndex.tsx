import React, {useCallback, useEffect} from 'react';
import {useSearchParams} from "react-router-dom";
import {useDispatch, useSelector} from "react-redux";
import {RootState} from "../../../redux/store";
import {todosActions} from "../../../redux/todos/todos.actions";
import {Col, Form, message, Row, Select, Switch, Table} from "antd";
import {ColumnsType} from "antd/es/table";
import {Todo} from "../../../gql/modules/todos/todos.types";
import {ButtonsVUR} from "../../../components/ButtonsVUD/ButtonsVUR";
import Title from 'antd/lib/typography/Title';
import {TodosCreate} from "../TodosCreate/TodosCreate";
import Search from "antd/es/input/Search";
import debounce from 'lodash.debounce';
import {ToDosSortOrder} from "../../../gql/enums/order";

export const TodosIndex = () => {
    const dispatch = useDispatch();
    const todos = useSelector((s: RootState) => s.todos.todos)
    const fetchTodosLoading = useSelector((s: RootState) => s.todos.fetchTodosLoading)
    const fetchTodosError = useSelector((s: RootState) => s.todos.fetchTodosError)
    const fetchRemoveTodoError = useSelector((s: RootState) => s.todos.fetchRemoveTodoError)
    const [searchParams, setSearchParams] = useSearchParams();

    useEffect(() => {
        if (fetchTodosError) {
            message.error(fetchTodosError);
            dispatch(todosActions.setFetchTodosError(''));
        }
        if (fetchRemoveTodoError) {
            message.error(fetchRemoveTodoError);
            dispatch(todosActions.setFetchRemoveTodoError(''));
        }
    }, [fetchTodosError, fetchRemoveTodoError])

    useEffect(() => {
        const likeInput = searchParams.get('like');
        const sortOrder = searchParams.get('sortOrder');
        const categoryIdString = searchParams.get('categoryId');
        const categoryId = categoryIdString ? parseInt(categoryIdString) : null;
        dispatch(todosActions.fetchTodos(likeInput, sortOrder, categoryId));
    }, [searchParams])

    const onRemove = (id: number): void => {
        dispatch(todosActions.fetchRemoveTodo(id));
    }

    const columns: ColumnsType<Todo> = [
        {
            title: 'Is complete',
            dataIndex: 'isComplete',
            key: 'isComplete',
            width: '110px',
            render: (text, todo) => (
                <Switch size="small" defaultChecked={todo.isComplete}/>
            ),
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Deadline',
            dataIndex: 'deadline',
            key: 'deadline',
        },
        {
            title: 'DateComplete',
            dataIndex: 'dateComplete',
            key: 'dateComplete',
        },
        {
            title: 'Category',
            dataIndex: 'category',
            key: 'category',
            render: (text, todo) => (
                <div>{todo.category?.name}</div>
            ),
        },
        {
            title: 'Actions',
            dataIndex: 'actions',
            key: 'actions',
            width: '130px',
            render: (text, todo) => (
                <ButtonsVUR updateUrl={`update/${todo.id}`} onRemove={() => onRemove(todo.id)}/>
            ),
        },
    ];

    const debouncedSearchTodosHandler = useCallback(debounce(like => setSearchParams({like}), 500), []);
    const searchTodosHandler = (value: string) => {
        debouncedSearchTodosHandler(value);
    };

    return (
        <div>
            <TodosCreate/>
            <Title level={2}>Todos</Title>
            <Form
                name="TodosFilterForm"
            >
                <Row>
                    <Col span={10}>
                        <Form.Item name="like">
                            <Search
                                allowClear
                                value={searchParams.get('like') || ''}
                                onChange={e => searchTodosHandler(e.target.value)}
                                placeholder="Search"
                                enterButton
                                loading={fetchTodosLoading}
                            />
                        </Form.Item>
                    </Col>
                    <Col span={7}>
                        <Form.Item name="Sort order">
                            <Select
                                value={searchParams.get('sortOrder') || ToDosSortOrder.deadlineAcs}
                                placeholder="Select a person"
                                onChange={sortOrder => setSearchParams({sortOrder})}
                            >
                                {(Object.keys(ToDosSortOrder) as Array<keyof typeof ToDosSortOrder>).map(key => (
                                    <Select.Option value={key}>{key}</Select.Option>
                                ))}
                            </Select>
                        </Form.Item>
                    </Col>
                    <Col span={7}>
                        <Form.Item name="categoryId">
                            <Select
                                value={searchParams.get('categoryId')}
                                placeholder="Category"
                                onChange={categoryId => setSearchParams({categoryId})}
                            >
                            </Select>
                        </Form.Item>
                    </Col>
                </Row>
            </Form>
            <Table
                rowKey={'id'}
                dataSource={todos}
                columns={columns}
                pagination={false}
                loading={fetchTodosLoading}
            />

        </div>
    );
};