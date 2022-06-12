import React, {useCallback, useEffect} from 'react';
import {useSearchParams} from "react-router-dom";
import {useDispatch, useSelector} from "react-redux";
import {RootState} from "../../../store/store";
import {todosActions} from "../../../store/todos/todos.actions";
import {Col, Form, message, Row, Select, Switch, Table} from "antd";
import {ColumnsType} from "antd/es/table";
import {Todo} from "../../../graphQL/modules/todos/todos.types";
import {ButtonsVUR} from "../../../components/ButtonsVUD/ButtonsVUR";
import Title from 'antd/lib/typography/Title';
import {TodosCreate} from "../TodosCreate/TodosCreate";
import Search from "antd/es/input/Search";
import debounce from 'lodash.debounce';
import {TodosSortOrder} from "../../../graphQL/enums/todosSortOrder";
import {categoriesActions} from "../../../store/categories/categories.actions";
import {camelCaseToString, stringToUSDatetime} from "../../../convertors/stringToDatetimeConvertors";
import {Loading} from "../../../components/Loading/Loading";

export const TodosIndex = () => {
    const dispatch = useDispatch();
    const categories = useSelector((s: RootState) => s.categories.categories)
    const fetchCategoriesLoading = useSelector((s: RootState) => s.categories.fetchCategoriesLoading)
    const fetchCategoriesError = useSelector((s: RootState) => s.categories.fetchCategoriesError)

    const pageSize = useSelector((s: RootState) => s.todos.pageSize)
    const total = useSelector((s: RootState) => s.todos.total)
    const todos = useSelector((s: RootState) => s.todos.todos)
    const fetchTodosLoading = useSelector((s: RootState) => s.todos.fetchTodosLoading)
    const fetchTodosError = useSelector((s: RootState) => s.todos.fetchTodosError)
    const fetchRemoveTodoError = useSelector((s: RootState) => s.todos.fetchRemoveTodoError)
    const [searchParams, setSearchParams] = useSearchParams();

    useEffect(() => {
        dispatch(categoriesActions.fetchCategories(1, null, null))
    }, [])

    useEffect(() => {
        const page = parseInt(searchParams.get('page') || '') || 1
        const likeInput = searchParams.get('like');
        const sortOrderString = searchParams.get('sortOrder');
        const sortOrder = TodosSortOrder[sortOrderString as keyof typeof TodosSortOrder] || TodosSortOrder.deadlineDecs;
        const categoryIdString = searchParams.get('categoryId');
        const categoryId = categoryIdString ? parseInt(categoryIdString) : null;
        dispatch(todosActions.fetchTodos(page, likeInput, sortOrder, categoryId));
    }, [searchParams])

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

    const onRemove = (id: number): void => {
        dispatch(todosActions.fetchRemoveTodo(id));
    }

    const switchIsCompleteHandler = (todo: Todo, isComplete: boolean): void => {
        dispatch(todosActions.fetchUpdateTodo({
            id: todo.id,
            isComplete: isComplete,
            name: todo.name,
            deadline: todo.deadline,
            categoryId: todo.categoryId,
        }))
    }

    const columns: ColumnsType<Todo> = [
        {
            title: 'Is complete',
            dataIndex: 'isComplete',
            key: 'isComplete',
            width: '110px',
            render: (text, todo) => (
                <Switch onChange={isComplete => switchIsCompleteHandler(todo, isComplete)} size="small"
                        defaultChecked={todo.isComplete}/>
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
            render: (text, todo) => (
                <div>{stringToUSDatetime(text)}</div>
            )
        },
        {
            title: 'DateComplete',
            dataIndex: 'dateComplete',
            key: 'dateComplete',
            render: (text, todo) => (
                <div>{stringToUSDatetime(text)}</div>
            )
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

    if (total === 0 && pageSize === 0 && fetchTodosLoading)
        return <Loading/>

    return (
        <div>
            <TodosCreate/>
            <Title level={2}>Todos</Title>
            <Form name="TodosFilterForm">
                <Row>
                    <Col span={10}>
                        <Form.Item name="like">
                            <Search
                                defaultValue={searchParams.get('like') || ''}
                                onChange={e => searchTodosHandler(e.target.value)}
                                placeholder="Search"
                                enterButton
                                loading={fetchTodosLoading}
                                allowClear={true}
                            />
                        </Form.Item>
                    </Col>
                    <Col span={7}>
                        <Form.Item name="Sort order">
                            <Select
                                defaultValue={searchParams.get('sortOrder') || TodosSortOrder.deadlineAcs}
                                placeholder="Sorting"
                                onChange={sortOrder => setSearchParams({sortOrder})}
                            >
                                {(Object.keys(TodosSortOrder) as Array<keyof typeof TodosSortOrder>).map(key => (
                                    <Select.Option value={key} key={key}>{camelCaseToString(key)}</Select.Option>
                                ))}
                            </Select>
                        </Form.Item>
                    </Col>
                    <Col span={7}>
                        <Form.Item name="categoryId">
                            <Select
                                defaultValue={searchParams.get('categoryId')}
                                placeholder="Category"
                                onChange={categoryId => setSearchParams({categoryId})}
                                loading={fetchCategoriesLoading}
                                allowClear={true}
                            >
                                {categories.map(category => (
                                    <Select.Option key={category.id} value={category.id}>{category.name}</Select.Option>
                                ))}
                            </Select>
                        </Form.Item>
                    </Col>
                </Row>
            </Form>
            <Table
                rowKey={'id'}
                dataSource={todos}
                columns={columns}
                loading={fetchTodosLoading}
                pagination={{
                    current: parseInt(searchParams.get('page') || '') || 1,
                    defaultPageSize: pageSize,
                    total: total,
                    onChange: page => setSearchParams({page: page.toString()}),
                }}
            />

        </div>
    );
};