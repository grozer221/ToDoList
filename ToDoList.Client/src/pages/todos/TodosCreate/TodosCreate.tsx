import React, {useEffect} from 'react';
import {useDispatch, useSelector} from "react-redux";
import {Col, DatePicker, Form, Input, message, Row, Select} from "antd";
import Title from "antd/lib/typography/Title";
import {ButtonSubmit} from "../../../components/ButtonSubmit/ButtonSubmit";
import {TodosCreateInputType} from "../../../graphQL/modules/todos/todos.mutations";
import {todosActions} from "../../../store/todos/todos.actions";
import {RootState} from "../../../store/store";

export const TodosCreate = () => {
    const categories = useSelector((s: RootState) => s.categories.categories)
    const fetchCreateTodoError = useSelector((s: RootState) => s.todos.fetchCreateTodoError)
    const fetchCreateTodoLoading = useSelector((s: RootState) => s.todos.fetchCreateTodoLoading)
    const dispatch = useDispatch();
    const [form] = Form.useForm();

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
        form.resetFields();
    }

    return (
        <Form
            name="TodoCreateForm"
            onFinish={onFinish}
            form={form}
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
                        <Select placeholder="Category" allowClear={true}>
                            {categories.map(category => (
                                <Select.Option key={category.id} value={category.id}>{category.name}</Select.Option>
                            ))}
                        </Select>
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