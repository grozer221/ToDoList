import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {Col, DatePicker, Form, Input, message, Row} from "antd";
import Title from "antd/lib/typography/Title";
import {ButtonSubmit} from "../../../components/ButtonSubmit/ButtonSubmit";
import {TodosCreateInputType} from "../../../gql/modules/todos/todos.mutations";
import {todosActions} from "../../../redux/todos/todos.actions";
import {RootState} from "../../../redux/store";

export const TodosCreate = () => {
    const fetchCreateTodoError = useSelector((s: RootState) => s.todos.fetchCreateTodoError)
    const fetchCreateTodoLoading = useSelector((s: RootState) => s.todos.fetchCreateTodoLoading)
    const dispatch = useDispatch();

    useEffect(() => {
        if (fetchCreateTodoError) {
            message.error(fetchCreateTodoError);
            dispatch(todosActions.setFetchCreateTodoError(''));
        }
    }, [fetchCreateTodoError])

    const onFinish = (toDosCreateInputType: TodosCreateInputType): void => {
        // @ts-ignore
        const deadline = toDosCreateInputType.deadline && new Date(toDosCreateInputType.deadline._d).toISOString();
        console.log({...toDosCreateInputType, deadline})
        dispatch(todosActions.fetchCreateTodo({...toDosCreateInputType, deadline}))
    }

    return (
        <Form
            name="TodoCreateForm"
            onFinish={onFinish}
        >
            <Title level={2}>Create todo</Title>
            <Row>
                <Col span={10}>
                    <Form.Item
                        name="name"
                        rules={[{required: true, message: 'Name is required!'}]}
                    >
                        <Input placeholder="Name"/>
                    </Form.Item>
                </Col>
                <Col span={7}>
                    <Form.Item
                        name="deadline"
                    >
                        <DatePicker showTime style={{width: '100%'}}/>
                    </Form.Item>
                </Col>
                <Col span={6}>
                    <Form.Item
                        name="categoryId"
                    >
                        <Input placeholder="Category" type={'number'}/>
                    </Form.Item>
                </Col>
                <Col span={1}>
                    <Form.Item>
                        <ButtonSubmit loading={fetchCreateTodoLoading} isSubmit={true}>Create</ButtonSubmit>
                    </Form.Item>
                </Col>
            </Row>
        </Form>
    );
};