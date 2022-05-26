import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {RootState} from "../../../redux/store";
import {useParams} from "react-router-dom";
import {todosActions} from "../../../redux/todos/todos.actions";
import {DatePicker, Form, Input, message, Switch} from 'antd';
import Title from "antd/lib/typography/Title";
import {ButtonSubmit} from "../../../components/ButtonSubmit/ButtonSubmit";
import {Loading} from "../../../components/Loading/Loading";
import {ToDosUpdateInputType} from "../../../gql/modules/todos/todos.mutations";
import moment from 'moment';

export const TodosUpdate = () => {
    const params = useParams();
    const inUpdateTodoId = parseInt(params.id || '') || 0;
    const inUpdateTodo = useSelector((s: RootState) => s.todos.inUpdateTodo)
    const fetchInUpdateTodoError = useSelector((s: RootState) => s.todos.fetchInUpdateTodoError)
    const fetchInUpdateTodoLoading = useSelector((s: RootState) => s.todos.fetchInUpdateTodoLoading)
    const fetchUpdateTodoError = useSelector((s: RootState) => s.todos.fetchUpdateTodoError)
    const fetchUpdateTodoLoading = useSelector((s: RootState) => s.todos.fetchUpdateTodoLoading)
    const todos = useSelector((s: RootState) => s.todos.todos)
    const dispatch = useDispatch();

    useEffect(() => {
        const updateTodo = todos.find(t => t.id === inUpdateTodoId);
        if (updateTodo) {
            dispatch(todosActions.setInUpdateTodo(updateTodo));
            dispatch(todosActions.setFetchInUpdateTodoLoading(false));
        } else {
            dispatch(todosActions.fetchInUpdateTodo(inUpdateTodoId));
        }

        return () => {
            dispatch(todosActions.resetAfterLeaveUpdatePage());
        }
    }, [])

    useEffect(() => {
        if (fetchInUpdateTodoError)
            message.error(fetchInUpdateTodoError);
        if (fetchUpdateTodoError)
            message.error(fetchUpdateTodoError)
    }, [fetchInUpdateTodoError, fetchUpdateTodoError])

    const onFinish = async (toDosUpdateInputType: ToDosUpdateInputType) => {
        // @ts-ignore
        const deadline = toDosUpdateInputType.deadline && new Date(toDosUpdateInputType.deadline._d).toISOString();
        console.log({...toDosUpdateInputType, deadline})
        dispatch(todosActions.fetchUpdateTodo({...toDosUpdateInputType, deadline}))
    };

    if (fetchInUpdateTodoLoading)
        return <Loading/>

    return (
        <Form
            name="TodoUpdateForm"
            onFinish={onFinish}
            initialValues={{
                id: inUpdateTodo?.id,
                isComplete: inUpdateTodo?.isComplete,
                name: inUpdateTodo?.name,
                deadline: inUpdateTodo?.deadline && moment(inUpdateTodo?.deadline),
            }}
        >
            <Title level={2}>Update todo</Title>
            <Form.Item name="id" style={{display: 'none'}}>
                <Input type={'hidden'}/>
            </Form.Item>
            <Form.Item
                name="isComplete"
                label="IsComplete"
            >
                <Switch size="small" defaultChecked={inUpdateTodo?.isComplete}/>
            </Form.Item>
            <Form.Item
                name="name"
                label="Name"
                rules={[{required: true, message: 'Name is required!'}]}
            >
                <Input placeholder="Name"/>
            </Form.Item>
            <Form.Item
                name="deadline"
                label="Deadline"
            >
                <DatePicker showTime/>
            </Form.Item>
            <Form.Item
                name="categoryId"
                label="Category"
            >
                <Input placeholder="Category" type={'number'}/>
            </Form.Item>
            <Form.Item>
                <ButtonSubmit loading={fetchUpdateTodoLoading} isSubmit={true}>Update</ButtonSubmit>
            </Form.Item>
        </Form>
    );
};